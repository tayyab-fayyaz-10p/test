﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SSH.Core.Resources {
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
    internal class Mail {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Mail() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SSH.Core.Resources.Mail", typeof(Mail).Assembly);
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
        
        /// <summary>
        ///   Looks up a localized string similar to Dear {0}, &lt;br&gt; &lt;br&gt; Welcome to FH - Customer Onboarding App.In order to continue, kindly download our App(URL) and enter the username &apos;{1}&apos; on the first screen to continue with further verification.&lt;br&gt;&lt;br&gt; Thank you..
        /// </summary>
        internal static string CreateUserEmailBody {
            get {
                return ResourceManager.GetString("CreateUserEmailBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome to FH - Customer Onboarding App.
        /// </summary>
        internal static string CreateUserEmailSubject {
            get {
                return ResourceManager.GetString("CreateUserEmailSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thank you for using SSH APP. {0} Payment successful processed.&lt;br&gt;&lt;br&gt;Regards,&lt;br&gt;-SSH.
        /// </summary>
        internal static string JobCompletedEmailBody {
            get {
                return ResourceManager.GetString("JobCompletedEmailBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Receipt - SSH.
        /// </summary>
        internal static string JobCompletedEmailSubject {
            get {
                return ResourceManager.GetString("JobCompletedEmailSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dear {0}, &lt;br&gt; &lt;br&gt;  Please note the meeting location as shown in image at the time of customer onboarding &lt;br&gt;&lt;br&gt; &lt;img src = &apos;{1}&apos; /&gt;.&lt;br&gt;&lt;br&gt; Thank you..
        /// </summary>
        internal static string LocationEmailBody {
            get {
                return ResourceManager.GetString("LocationEmailBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Meeting Location.
        /// </summary>
        internal static string LocationEmailSubject {
            get {
                return ResourceManager.GetString("LocationEmailSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dear {0}, &lt;br&gt;&lt;br&gt; Your One Time Pin is {1}. In order to proceed to SSH App, kindly enter this pin number in your mobile device. &lt;br&gt;&lt;br&gt; Thank you..
        /// </summary>
        internal static string OTPBody {
            get {
                return ResourceManager.GetString("OTPBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One Time Pin.
        /// </summary>
        internal static string OTPSubject {
            get {
                return ResourceManager.GetString("OTPSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dear applicant, thank you for applying to SSH Partner. Unfortunately we are unable to proceed with your application now due to {0}. We wish you the very best..
        /// </summary>
        internal static string UserRejectionEmailBody {
            get {
                return ResourceManager.GetString("UserRejectionEmailBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Application Status.
        /// </summary>
        internal static string UserRejectionEmailSubject {
            get {
                return ResourceManager.GetString("UserRejectionEmailSubject", resourceCulture);
            }
        }
    }
}