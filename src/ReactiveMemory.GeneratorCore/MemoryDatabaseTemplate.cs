﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ReactiveMemory.GeneratorCore
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class MemoryDatabaseTemplate : MemoryDatabaseTemplateBase
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("// <auto-generated />\r\n#pragma warning disable CS0105\r\n");
            this.Write(this.ToStringHelper.ToStringWithCulture(Using));
            this.Write("\r\n\r\nnamespace ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            this.Write("\r\n{\r\n   public interface I");
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            this.Write("\r\n   {\r\n        public IObservable<EntityChange<TEntity>> OnChange<TEntity>();\r\n");
 foreach(var item in GenerationContexts) { 
            this.Write("        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table { get; }\r\n");
 } 
            this.Write("   }\r\n\r\n   public sealed class ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            this.Write(" : MemoryDatabaseBase, I");
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            this.Write("\r\n   {\r\n");
 foreach(var item in GenerationContexts) { 
            this.Write("        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table { get; private set; }\r\n");
 } 
            this.Write("\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            this.Write("(\r\n");
 for(var i = 0; i < GenerationContexts.Length; i++) { var item = GenerationContexts[i]; 
            this.Write("            ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table");
            this.Write(this.ToStringHelper.ToStringWithCulture((i == GenerationContexts.Length - 1) ? "" : ","));
            this.Write("\r\n");
 } 
            this.Write("        , ChangesConveyor changesConveyor) : base(changesConveyor)\r\n        {\r\n");
 for(var i = 0; i < GenerationContexts.Length; i++) { var item = GenerationContexts[i]; 
            this.Write("            this.");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table;\r\n");
 } 
            this.Write("        }\r\n\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            this.Write(@"(byte[] databaseBinary, IChangesMediatorFactory changesMediatorFactory, bool internString = true, MessagePack.IFormatterResolver formatterResolver = null, int maxDegreeOfParallelism = 1)
            : base(databaseBinary, changesMediatorFactory, internString, formatterResolver, maxDegreeOfParallelism)
        {
        }

        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            this.Write(@"(byte[] databaseBinary, ChangesConveyor changesConveyor, bool internString = true, MessagePack.IFormatterResolver formatterResolver = null, int maxDegreeOfParallelism = 1)
            : base(databaseBinary, changesConveyor, internString, formatterResolver, maxDegreeOfParallelism)
        {
        }

        protected override void Init(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            if(maxDegreeOfParallelism == 1)
            {
                InitSequential(header, databaseBinary, options, maxDegreeOfParallelism);
            }
            else
            {
                InitParallel(header, databaseBinary, options, maxDegreeOfParallelism);
            }
        }

        void InitSequential(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
");
 foreach(var item in GenerationContexts) { 
            this.Write("            this.");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table = ExtractTableData<");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write(", ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table>(header, databaseBinary, options, xs => new ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table(xs));\r\n");
 } 
            this.Write(@"        }

        void InitParallel(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            var extracts = new Action[]
            {
");
 foreach(var item in GenerationContexts) { 
            this.Write("                () => this.");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table = ExtractTableData<");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write(", ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table>(header, databaseBinary, options, xs => new ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table(xs)),\r\n");
 } 
            this.Write(@"            };
            
            System.Threading.Tasks.Parallel.Invoke(new System.Threading.Tasks.ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            }, extracts);
        }

        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(PrefixClassName));
            this.Write("Transaction BeginTransaction()\r\n        {\r\n            return new ");
            this.Write(this.ToStringHelper.ToStringWithCulture(PrefixClassName));
            this.Write("Transaction(this);\r\n        }\r\n\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(PrefixClassName));
            this.Write("DatabaseBuilder ToDatabaseBuilder()\r\n        {\r\n            var builder = new ");
            this.Write(this.ToStringHelper.ToStringWithCulture(PrefixClassName));
            this.Write("DatabaseBuilder();\r\n");
 foreach(var item in GenerationContexts) { 
            this.Write("            builder.Append(this.");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table.GetRawDataUnsafe());\r\n");
 } 
            this.Write("            return builder;\r\n        }\r\n\r\n        public ");
            this.Write(this.ToStringHelper.ToStringWithCulture(PrefixClassName));
            this.Write("DatabaseBuilder ToDatabaseBuilder(MessagePack.IFormatterResolver resolver)\r\n     " +
                    "   {\r\n            var builder = new ");
            this.Write(this.ToStringHelper.ToStringWithCulture(PrefixClassName));
            this.Write("DatabaseBuilder(resolver);\r\n");
 foreach(var item in GenerationContexts) { 
            this.Write("            builder.Append(this.");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table.GetRawDataUnsafe());\r\n");
 } 
            this.Write(@"            return builder;
        }

#if !DISABLE_MASTERMEMORY_VALIDATOR

        public ValidateResult Validate()
        {
            var result = new ValidateResult();
            var database = new ValidationDatabase(new object[]
            {
");
 foreach(var item in GenerationContexts) { 
            this.Write("                ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table,\r\n");
 } 
            this.Write("            });\r\n\r\n");
 foreach(var item in GenerationContexts) { 
            this.Write("            ((ITableUniqueValidate)");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table).ValidateUnique(result);\r\n            ValidateTable(");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table.All, database, \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.PrimaryKey.BuildPropertyTupleName()));
            this.Write("\", ");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table.PrimaryKeySelector, result);\r\n");
 } 
            this.Write("\r\n            return result;\r\n        }\r\n\r\n#endif\r\n\r\n        static ReactiveMemor" +
                    "y.Meta.MetaDatabase metaTable;\r\n\r\n        public static object GetTable(");
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            this.Write(" db, string tableName)\r\n        {\r\n            switch (tableName)\r\n            {\r" +
                    "\n");
 foreach(var item in GenerationContexts) { 
            this.Write("                case \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.MemoryTableName));
            this.Write("\":\r\n                    return db.");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table;\r\n");
 } 
            this.Write(@"                
                default:
                    return null;
            }
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static ReactiveMemory.Meta.MetaDatabase GetMetaDatabase()
        {
            if (metaTable != null) return metaTable;

            var dict = new Dictionary<string, ReactiveMemory.Meta.MetaTable>();
");
 foreach(var item in GenerationContexts) { 
            this.Write("            dict.Add(\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.MemoryTableName));
            this.Write("\", ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            this.Write(".Tables.");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ClassName));
            this.Write("Table.CreateMetaTable());\r\n");
 } 
            this.Write("\r\n            metaTable = new ReactiveMemory.Meta.MetaDatabase(dict);\r\n          " +
                    "  return metaTable;\r\n        }\r\n\r\n#endif\r\n    }\r\n}");
            return this.GenerationEnvironment.ToString();
        }
    }
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class MemoryDatabaseTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        public System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
