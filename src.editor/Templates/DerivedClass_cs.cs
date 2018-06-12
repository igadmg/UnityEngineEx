﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnityEditorEx.src.editor.Templates {
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    
    public partial class DerivedClass_cs : DerivedClass_csBase {
        
        
        private string _namespacenameField;

        public string namespacename {
            get {
                return this._namespacenameField;
            }
        }

        private string _classnameField;

        public string classname {
            get {
                return this._classnameField;
            }
        }

        private string _baseclassnameField;

        public string baseclassname {
            get {
                return this._baseclassnameField;
            }
        }

        
        public virtual string TransformText() {
            this.GenerationEnvironment = null;
            
            #line 10 ""
            this.Write("using System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\nus" +
                    "ing UnityEngineEx;\n\n\n\nnamespace ");
            
            #line default
            #line hidden
            
            #line 17 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( this.namespacename ));
            
            #line default
            #line hidden
            
            #line 17 ""
            this.Write("\n{\n    public class ");
            
            #line default
            #line hidden
            
            #line 19 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( this.classname ));
            
            #line default
            #line hidden
            
            #line 19 ""
            this.Write(" : ");
            
            #line default
            #line hidden
            
            #line 19 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( this.baseclassname ));
            
            #line default
            #line hidden
            
            #line 19 ""
            this.Write("\n    {\n    }\n}\n");
            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
        
        public virtual void Initialize() {
            if ((this.Errors.HasErrors == false)) {
                bool _namespacenameAcquired = false;
                if (((this.Session != null) 
                            && this.Session.ContainsKey("namespacename"))) {
                    object data = this.Session["namespacename"];
                    if (typeof(string).IsAssignableFrom(data.GetType())) {
                        this._namespacenameField = ((string)(data));
                        _namespacenameAcquired = true;
                    }
                    else {
                        this.Error("The type \'System.String\' of the parameter \'namespacename\' did not match the type " +
                                "passed to the template");
                    }
                }
                if ((_namespacenameAcquired == false)) {
                    object data = System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("namespacename");
                    if ((data != null)) {
                        if (typeof(string).IsAssignableFrom(data.GetType())) {
                            this._namespacenameField = ((string)(data));
                            _namespacenameAcquired = true;
                        }
                        else {
                            this.Error("The type \'System.String\' of the parameter \'namespacename\' did not match the type " +
                                    "passed to the template");
                        }
                    }
                }
                bool _classnameAcquired = false;
                if (((this.Session != null) 
                            && this.Session.ContainsKey("classname"))) {
                    object data = this.Session["classname"];
                    if (typeof(string).IsAssignableFrom(data.GetType())) {
                        this._classnameField = ((string)(data));
                        _classnameAcquired = true;
                    }
                    else {
                        this.Error("The type \'System.String\' of the parameter \'classname\' did not match the type pass" +
                                "ed to the template");
                    }
                }
                if ((_classnameAcquired == false)) {
                    object data = System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("classname");
                    if ((data != null)) {
                        if (typeof(string).IsAssignableFrom(data.GetType())) {
                            this._classnameField = ((string)(data));
                            _classnameAcquired = true;
                        }
                        else {
                            this.Error("The type \'System.String\' of the parameter \'classname\' did not match the type pass" +
                                    "ed to the template");
                        }
                    }
                }
                bool _baseclassnameAcquired = false;
                if (((this.Session != null) 
                            && this.Session.ContainsKey("baseclassname"))) {
                    object data = this.Session["baseclassname"];
                    if (typeof(string).IsAssignableFrom(data.GetType())) {
                        this._baseclassnameField = ((string)(data));
                        _baseclassnameAcquired = true;
                    }
                    else {
                        this.Error("The type \'System.String\' of the parameter \'baseclassname\' did not match the type " +
                                "passed to the template");
                    }
                }
                if ((_baseclassnameAcquired == false)) {
                    object data = System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("baseclassname");
                    if ((data != null)) {
                        if (typeof(string).IsAssignableFrom(data.GetType())) {
                            this._baseclassnameField = ((string)(data));
                            _baseclassnameAcquired = true;
                        }
                        else {
                            this.Error("The type \'System.String\' of the parameter \'baseclassname\' did not match the type " +
                                    "passed to the template");
                        }
                    }
                }
            }

        }
    }
    
    public class DerivedClass_csBase {
        
        private global::System.Text.StringBuilder builder;
        
        private global::System.Collections.Generic.IDictionary<string, object> session;
        
        private global::System.CodeDom.Compiler.CompilerErrorCollection errors;
        
        private string currentIndent = string.Empty;
        
        private global::System.Collections.Generic.Stack<int> indents;
        
        private ToStringInstanceHelper _toStringHelper = new ToStringInstanceHelper();
        
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session {
            get {
                return this.session;
            }
            set {
                this.session = value;
            }
        }
        
        public global::System.Text.StringBuilder GenerationEnvironment {
            get {
                if ((this.builder == null)) {
                    this.builder = new global::System.Text.StringBuilder();
                }
                return this.builder;
            }
            set {
                this.builder = value;
            }
        }
        
        protected global::System.CodeDom.Compiler.CompilerErrorCollection Errors {
            get {
                if ((this.errors == null)) {
                    this.errors = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errors;
            }
        }
        
        public string CurrentIndent {
            get {
                return this.currentIndent;
            }
        }
        
        private global::System.Collections.Generic.Stack<int> Indents {
            get {
                if ((this.indents == null)) {
                    this.indents = new global::System.Collections.Generic.Stack<int>();
                }
                return this.indents;
            }
        }
        
        public ToStringInstanceHelper ToStringHelper {
            get {
                return this._toStringHelper;
            }
        }
        
        public void Error(string message) {
            this.Errors.Add(new global::System.CodeDom.Compiler.CompilerError(null, -1, -1, null, message));
        }
        
        public void Warning(string message) {
            global::System.CodeDom.Compiler.CompilerError val = new global::System.CodeDom.Compiler.CompilerError(null, -1, -1, null, message);
            val.IsWarning = true;
            this.Errors.Add(val);
        }
        
        public string PopIndent() {
            if ((this.Indents.Count == 0)) {
                return string.Empty;
            }
            int lastPos = (this.currentIndent.Length - this.Indents.Pop());
            string last = this.currentIndent.Substring(lastPos);
            this.currentIndent = this.currentIndent.Substring(0, lastPos);
            return last;
        }
        
        public void PushIndent(string indent) {
            this.Indents.Push(indent.Length);
            this.currentIndent = (this.currentIndent + indent);
        }
        
        public void ClearIndent() {
            this.currentIndent = string.Empty;
            this.Indents.Clear();
        }
        
        public void Write(string textToAppend) {
            this.GenerationEnvironment.Append(textToAppend);
        }
        
        public void Write(string format, params object[] args) {
            this.GenerationEnvironment.AppendFormat(format, args);
        }
        
        public void WriteLine(string textToAppend) {
            this.GenerationEnvironment.Append(this.currentIndent);
            this.GenerationEnvironment.AppendLine(textToAppend);
        }
        
        public void WriteLine(string format, params object[] args) {
            this.GenerationEnvironment.Append(this.currentIndent);
            this.GenerationEnvironment.AppendFormat(format, args);
            this.GenerationEnvironment.AppendLine();
        }
        
        public class ToStringInstanceHelper {
            
            private global::System.IFormatProvider formatProvider = global::System.Globalization.CultureInfo.InvariantCulture;
            
            public global::System.IFormatProvider FormatProvider {
                get {
                    return this.formatProvider;
                }
                set {
                    if ((value != null)) {
                        this.formatProvider = value;
                    }
                }
            }
            
            public string ToStringWithCulture(object objectToConvert) {
                if ((objectToConvert == null)) {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                global::System.Type type = objectToConvert.GetType();
                global::System.Type iConvertibleType = typeof(global::System.IConvertible);
                if (iConvertibleType.IsAssignableFrom(type)) {
                    return ((global::System.IConvertible)(objectToConvert)).ToString(this.formatProvider);
                }
                global::System.Reflection.MethodInfo methInfo = type.GetMethod("ToString", new global::System.Type[] {
                            iConvertibleType});
                if ((methInfo != null)) {
                    return ((string)(methInfo.Invoke(objectToConvert, new object[] {
                                this.formatProvider})));
                }
                return objectToConvert.ToString();
            }
        }
    }
}