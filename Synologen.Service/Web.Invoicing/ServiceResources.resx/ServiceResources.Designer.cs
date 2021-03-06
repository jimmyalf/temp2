﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Synologen.Service.Web.Invoicing.ServiceResources.resx {
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
    internal class ServiceResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ServiceResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Synologen.Service.Web.Invoicing.ServiceResources.resx.ServiceResources", typeof(ServiceResources).Assembly);
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
        ///   Looks up a localized string similar to Fakturor som skickades {Sent-Invoices}
        ///Fakturor som inte kunde skickas {Not-Sent-Invoices}.
        /// </summary>
        internal static string BatchInvoiceFailureEmailBody {
            get {
                return ResourceManager.GetString("BatchInvoiceFailureEmailBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Synologen Faktureringskörning {Date-Time} misslyckades.
        /// </summary>
        internal static string BatchInvoiceFailureEmailSubject {
            get {
                return ResourceManager.GetString("BatchInvoiceFailureEmailSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fakturor som skickades {Sent-Invoices}.
        /// </summary>
        internal static string BatchInvoiceSuccessEmailBody {
            get {
                return ResourceManager.GetString("BatchInvoiceSuccessEmailBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Synologen Faktureringskörning {Date-Time} lyckades.
        /// </summary>
        internal static string BatchInvoiceSuccessEmailSubject {
            get {
                return ResourceManager.GetString("BatchInvoiceSuccessEmailSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ett fel har uppstått i Synologen Webservice.
        /// </summary>
        internal static string ErrorEmailSubject {
            get {
                return ResourceManager.GetString("ErrorEmailSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Faktura {0} skickades via FTP: {1}.
        /// </summary>
        internal static string InvoiceSentHistoryMessage {
            get {
                return ResourceManager.GetString("InvoiceSentHistoryMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ordern importerades till visma och fick ordernr {0}. Pris inkl moms {1}, exkl moms {2}..
        /// </summary>
        internal static string OrderAddedToVismaHistoryMessage {
            get {
                return ResourceManager.GetString("OrderAddedToVismaHistoryMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Faktura-status för faktura {0} uppdaterades..
        /// </summary>
        internal static string OrderStatusUpdatedHistoryMessage {
            get {
                return ResourceManager.GetString("OrderStatusUpdatedHistoryMessage", resourceCulture);
            }
        }
    }
}
