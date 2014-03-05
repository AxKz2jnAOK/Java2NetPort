using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.JNDI
{
    public enum NameSyntaxKeys
    {
        /// <summary>
        /// Direction for parsing ("right_to_left", "left_to_right", "flat"). If unspecified, defaults to "flat", which means the namespace is flat with no hierarchical structure. 
        /// </summary>
        direction,
        /// <summary>
        /// Separator between atomic name components. Required unless direction is "flat". 
        /// </summary>
        separator,
        /// <summary>
        /// If present, "true" means ignore the case when comparing name components. If its value is not "true", or if the property is not present, case is considered when comparing name components. 
        /// </summary>
        ignorecase,
        /// <summary>
        /// If present, specifies the escape string for overriding separator, escapes and quotes. 
        /// </summary>
        escape,
        /// <summary>
        /// If present, specifies the string delimiting start of a quoted string. 
        /// </summary>
        beginquote,
        /// <summary>
        /// String delimiting end of quoted string. If present, specifies the string delimiting the end of a quoted string. If not present, use syntax.beginquote as end quote. 
        /// </summary>
        endquote, 
        /// <summary>
        /// Alternative set of begin/end quotes. 
        /// </summary>
        beginquote2, 
        /// <summary>
        /// Alternative set of begin/end quotes. 
        /// </summary>        
        endquote2, 
        /// <summary>
        /// If present, "true" means trim any leading and trailing whitespaces in a name component for comparison purposes. If its value is not "true", or if the property is not present, blanks are significant. 
        /// </summary>
        trimblanks,
        /// <summary>
        /// If present, specifies the string that separates attribute-value-assertions when specifying multiple attribute/value pairs. (e.g. "," in age=65,gender=male). 
        /// </summary>
        ava,
        /// <summary>
        /// If present, specifies the string that separators attribute from value (e.g. "=" in "age=65") 
        /// </summary>
        typeval
    }
}
