﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Noxico.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Noxico.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static System.Drawing.Icon app {
            get {
                object obj = ResourceManager.GetObject("app", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap CharacterGenerator {
            get {
                object obj = ResourceManager.GetObject("CharacterGenerator", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;!DOCTYPE cultures [
        ///	&lt;!ELEMENT cultures (culture+)&gt;
        ///	&lt;!ELEMENT culture (cultureinfo|namegen)*&gt;
        ///	&lt;!ELEMENT cultureinfo (bodyplans|monogamous|marriage)*&gt;
        ///	&lt;!ELEMENT bodyplans (plan+)&gt;
        ///	&lt;!ELEMENT plan EMPTY&gt;
        ///	&lt;!ELEMENT monogamous (#PCDATA)&gt;
        ///	&lt;!ELEMENT marriage (#PCDATA)&gt;
        ///	&lt;!ELEMENT namegen (set|male|female|surname|town)*&gt;
        ///	&lt;!ELEMENT set (#PCDATA)&gt;
        ///	&lt;!ELEMENT male (illegal|rules)*&gt;
        ///	&lt;!ELEMENT female (illegal|rules)*&gt;
        ///	&lt;!ELEMENT surname (patronymic|illegal|ru [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Cultures {
            get {
                return ResourceManager.GetString("Cultures", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ; This is the default configuration for Noxico. Feel free to adjust this as you see fit.
        ///
        ///[video]
        ///;     tileset: Specifies which PNG file to use for the regular 256 characters.
        ///;              Should be a tileset with 32 glyphs per row, 8 rows. Actual window
        ///;              size is directly affected by this file. If the file doesn&apos;t exist,
        ///;              a built-in tileset is used.
        ///;     extiles: Basically the same deal as the tileset value, but for extra characters
        ///;              beyond the usual ran [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string DefaultSettings {
            get {
                return ResourceManager.GetString("DefaultSettings", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap ExtendedTiles {
            get {
                object obj = ResourceManager.GetObject("ExtendedTiles", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;!DOCTYPE colortable [
        ///	&lt;!ELEMENT colortable (color*)&gt;
        ///	&lt;!ELEMENT color EMPTY&gt;
        ///	&lt;!ATTLIST color name ID #REQUIRED rgb CDATA #REQUIRED cga CDATA #IMPLIED&gt;
        ///]&gt;
        ///&lt;colortable&gt;
        ///	&lt;!-- Skintones --&gt;
        ///	&lt;color name=&quot;Light&quot; rgb=&quot;246,225,214&quot; cga=&quot;15&quot; /&gt;
        ///	&lt;color name=&quot;Dark&quot; rgb=&quot;133,69,67&quot; cga=&quot;6&quot; /&gt;
        ///	&lt;color name=&quot;Ebony&quot; rgb=&quot;23,11,11&quot; cga=&quot;0&quot; /&gt;
        ///
        ///	&lt;!-- Hair colors --&gt;
        ///	&lt;color name=&quot;Blond&quot; rgb=&quot;198,167,94&quot; cga=&quot;6&quot; /&gt;
        ///	&lt;color name=&quot;Auburn&quot; rgb=&quot;111,53,26&quot; cga=&quot;6&quot; /&gt;
        ///        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string KnownColors {
            get {
                return ResourceManager.GetString("KnownColors", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap Tileset {
            get {
                object obj = ResourceManager.GetObject("Tileset", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap TitleScreen {
            get {
                object obj = ResourceManager.GetObject("TitleScreen", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static byte[] WallLookup {
            get {
                object obj = ResourceManager.GetObject("WallLookup", resourceCulture);
                return ((byte[])(obj));
            }
        }
    }
}
