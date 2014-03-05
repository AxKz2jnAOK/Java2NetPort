using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Java2NetPort.Attributes
{

    [Serializable]
    //[AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class SessionBeanAttributeBase : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (!args.Method.IsConstructor)
            {
                TransactionAttribute ta = (TransactionAttribute)args.Method.GetCustomAttributes(typeof(TransactionAttribute), true).FirstOrDefault();
                if(ta != null)
                {
                    if(ta.TransactionAttributeType == TransactionAttributeType.MANDATORY)
                    {
                        if (System.Transactions.Transaction.Current == null)
                        {
                            throw new TransactionRequiredException();
                        }
                    }
                    else if(ta.TransactionAttributeType == TransactionAttributeType.REQUIRED)
                    {
                        EJBTransactionScope ts2 = new EJBTransactionScope();

                        if (EJBContainer.Instance.TransactionScopes.Any())
                        {
                            EJBTransactionScope ts = EJBContainer.Instance.TransactionScopes.Peek();
                            
                            ts2.TransactionScope = new TransactionScope(TransactionScopeOption.Required);
                            ts2.TransactionAttributeType = TransactionAttributeType.REQUIRED;
                            ts2.DbContext = ts.DbContext;                            
                        }
                        else
                        {
                            ts2.TransactionScope = new TransactionScope(TransactionScopeOption.Required);
                            ts2.TransactionAttributeType = TransactionAttributeType.REQUIRED;
                            ts2.DbContext = EJBContainer.Instance.GetNewDbContext();
                        }
                        EJBContainer.Instance.TransactionScopes.Push(ts2);
                    }
                    else if(ta.TransactionAttributeType == TransactionAttributeType.REQUIRES_NEW)
                    {
                        EJBTransactionScope ts2 = new EJBTransactionScope();

                        if(EJBContainer.Instance.TransactionScopes.Any())
                        {
                            EJBTransactionScope ts = EJBContainer.Instance.TransactionScopes.Peek();
                            if(ts.TransactionAttributeType == TransactionAttributeType.REQUIRED)
                            {                                
                                ts2.TransactionScope         = new TransactionScope(TransactionScopeOption.RequiresNew);
                                ts2.TransactionAttributeType = TransactionAttributeType.REQUIRES_NEW;
                                ts2.DbContext                = EJBContainer.Instance.GetNewDbContext();                                
                            }
                            else
                            {
                                ts2.TransactionScope = new TransactionScope(TransactionScopeOption.Required);
                                ts2.TransactionAttributeType = TransactionAttributeType.REQUIRES_NEW;
                                ts2.DbContext = ts.DbContext;
                            }
                        }
                        else
                        {
                            ts2.TransactionScope = new TransactionScope(TransactionScopeOption.Required);
                            ts2.TransactionAttributeType = TransactionAttributeType.REQUIRES_NEW;
                            ts2.DbContext = EJBContainer.Instance.GetNewDbContext();                                
                        }
                        EJBContainer.Instance.TransactionScopes.Push(ts2);
                    }
                    else if (ta.TransactionAttributeType == TransactionAttributeType.SUPPORTS)
                    {
                        EJBTransactionScope ts2 = new EJBTransactionScope();

                        if (EJBContainer.Instance.TransactionScopes.Any())
                        {
                            EJBTransactionScope ts = EJBContainer.Instance.TransactionScopes.Peek();

                            ts2.TransactionScope = new TransactionScope(TransactionScopeOption.Required);
                            ts2.TransactionAttributeType = TransactionAttributeType.SUPPORTS;
                            ts2.DbContext = ts.DbContext;
                        }
                        else
                        {
                            ts2.TransactionScope = null;
                            ts2.TransactionAttributeType = TransactionAttributeType.SUPPORTS;
                            ts2.DbContext = EJBContainer.Instance.GetNewDbContext();
                        }
                        EJBContainer.Instance.TransactionScopes.Push(ts2);
                    }
                    else if (ta.TransactionAttributeType == TransactionAttributeType.NOT_SUPPORTED)
                    {
                        EJBTransactionScope ts2 = new EJBTransactionScope();

                        if (EJBContainer.Instance.TransactionScopes.Any())
                        {
                            EJBTransactionScope ts = EJBContainer.Instance.TransactionScopes.Peek();

                            ts2.TransactionScope = new TransactionScope(TransactionScopeOption.Suppress);
                            ts2.TransactionAttributeType = TransactionAttributeType.NOT_SUPPORTED;
                            ts2.DbContext = EJBContainer.Instance.GetNewDbContext();
                        }
                        else
                        {
                            ts2.TransactionScope = null;
                            ts2.TransactionAttributeType = TransactionAttributeType.NOT_SUPPORTED;
                            ts2.DbContext = EJBContainer.Instance.GetNewDbContext();
                        }
                        EJBContainer.Instance.TransactionScopes.Push(ts2);
                    }
                }
                Trace.WriteLine(string.Format("Entering {0}.{1}.", args.Method.DeclaringType.Name, args.Method.Name));
            }
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            if (!args.Method.IsConstructor)
            {
                TransactionAttribute ta = (TransactionAttribute)args.Method.GetCustomAttributes(typeof(TransactionAttribute), true).FirstOrDefault();
                if(ta != null)
                {
                    if(ta.TransactionAttributeType == TransactionAttributeType.MANDATORY)
                    {                        
                    }
                    else if(ta.TransactionAttributeType == TransactionAttributeType.REQUIRED                            
                            || ta.TransactionAttributeType == TransactionAttributeType.SUPPORTS)                            
                    {
                        EJBContainer c = EJBContainer.Instance;

                        EJBTransactionScope ts = EJBContainer.Instance.TransactionScopes.Pop();
                        if (!EJBContainer.Instance.TransactionScopes.Any())
                        {
                            if (c.EntityManager != null)
                            {
                                ts.DbContext.SaveChanges();
                            }
                        }

                        if (ts.TransactionScope != null)
                        {
                            ts.TransactionScope.Complete();
                            ts.TransactionScope.Dispose();
                            ts.TransactionScope = null;
                        }
                        ts.DbContext = null;
                        ts = null;
                    }                    
                    else if(ta.TransactionAttributeType == TransactionAttributeType.REQUIRES_NEW
                            || ta.TransactionAttributeType == TransactionAttributeType.NOT_SUPPORTED)
                    {
                        EJBContainer c = EJBContainer.Instance;

                        EJBTransactionScope ts = EJBContainer.Instance.TransactionScopes.Pop();
                        
                        if (c.EntityManager != null)
                        {
                            ts.DbContext.SaveChanges();
                        }
                        

                        if (ts.TransactionScope != null)
                        {
                            ts.TransactionScope.Complete();
                            ts.TransactionScope.Dispose();
                            ts.TransactionScope = null;
                        }
                        ts.DbContext = null;
                        ts = null;
                    }

                }
                Trace.WriteLine(string.Format("Entering {0}.{1}.", args.Method.DeclaringType.Name, args.Method.Name));
            }        
        } 
    }
}
