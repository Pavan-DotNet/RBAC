﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MOCDIntegrations.AJMMUNService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.informatica.com/dis/ws/", ConfigurationName="AJMMUNService.AJM_MPD_getResidentialGrantStatusbyEIDPortType")]
    public interface AJM_MPD_getResidentialGrantStatusbyEIDPortType {
        
        // CODEGEN: Generating message contract since the operation GetResidentialGrantStatus is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Output GetResidentialGrantStatus(MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Input request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Output> GetResidentialGrantStatusAsync(MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Input request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.informatica.com/dis/ws/")]
    public partial class GetResidentialGrantStatus : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string emiratesIdField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string EmiratesId {
            get {
                return this.emiratesIdField;
            }
            set {
                this.emiratesIdField = value;
                this.RaisePropertyChanged("EmiratesId");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.informatica.com/dis/ws/")]
    public partial class GetResidentialGrantStatusResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private GetResidentialGrantStatusResponseResidentialGrantStatus[] residentialGrantStatusField;
        
        private GetResidentialGrantStatusResponseError errorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ResidentialGrantStatus", Order=0)]
        public GetResidentialGrantStatusResponseResidentialGrantStatus[] ResidentialGrantStatus {
            get {
                return this.residentialGrantStatusField;
            }
            set {
                this.residentialGrantStatusField = value;
                this.RaisePropertyChanged("ResidentialGrantStatus");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public GetResidentialGrantStatusResponseError Error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
                this.RaisePropertyChanged("Error");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.informatica.com/dis/ws/")]
    public partial class GetResidentialGrantStatusResponseResidentialGrantStatus : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string typeOfLandField;
        
        private string residentialGrantedField;
        
        private string parcelIDField;
        
        private string sitePlanIssuedDateField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string TypeOfLand {
            get {
                return this.typeOfLandField;
            }
            set {
                this.typeOfLandField = value;
                this.RaisePropertyChanged("TypeOfLand");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ResidentialGranted {
            get {
                return this.residentialGrantedField;
            }
            set {
                this.residentialGrantedField = value;
                this.RaisePropertyChanged("ResidentialGranted");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ParcelID {
            get {
                return this.parcelIDField;
            }
            set {
                this.parcelIDField = value;
                this.RaisePropertyChanged("ParcelID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string SitePlanIssuedDate {
            get {
                return this.sitePlanIssuedDateField;
            }
            set {
                this.sitePlanIssuedDateField = value;
                this.RaisePropertyChanged("SitePlanIssuedDate");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.informatica.com/dis/ws/")]
    public partial class GetResidentialGrantStatusResponseError : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string inputParameterField;
        
        private string errorCodeField;
        
        private string errorMessageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string InputParameter {
            get {
                return this.inputParameterField;
            }
            set {
                this.inputParameterField = value;
                this.RaisePropertyChanged("InputParameter");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ErrorCode {
            get {
                return this.errorCodeField;
            }
            set {
                this.errorCodeField = value;
                this.RaisePropertyChanged("ErrorCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ErrorMessage {
            get {
                return this.errorMessageField;
            }
            set {
                this.errorMessageField = value;
                this.RaisePropertyChanged("ErrorMessage");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetResidentialGrantStatus_Input {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.informatica.com/dis/ws/", Order=0)]
        public MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus GetResidentialGrantStatus;
        
        public GetResidentialGrantStatus_Input() {
        }
        
        public GetResidentialGrantStatus_Input(MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus GetResidentialGrantStatus) {
            this.GetResidentialGrantStatus = GetResidentialGrantStatus;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetResidentialGrantStatus_Output {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.informatica.com/dis/ws/", Order=0)]
        public MOCDIntegrations.AJMMUNService.GetResidentialGrantStatusResponse GetResidentialGrantStatusResponse;
        
        public GetResidentialGrantStatus_Output() {
        }
        
        public GetResidentialGrantStatus_Output(MOCDIntegrations.AJMMUNService.GetResidentialGrantStatusResponse GetResidentialGrantStatusResponse) {
            this.GetResidentialGrantStatusResponse = GetResidentialGrantStatusResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AJM_MPD_getResidentialGrantStatusbyEIDPortTypeChannel : MOCDIntegrations.AJMMUNService.AJM_MPD_getResidentialGrantStatusbyEIDPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AJM_MPD_getResidentialGrantStatusbyEIDPortTypeClient : System.ServiceModel.ClientBase<MOCDIntegrations.AJMMUNService.AJM_MPD_getResidentialGrantStatusbyEIDPortType>, MOCDIntegrations.AJMMUNService.AJM_MPD_getResidentialGrantStatusbyEIDPortType {
        
        public AJM_MPD_getResidentialGrantStatusbyEIDPortTypeClient() {
        }
        
        public AJM_MPD_getResidentialGrantStatusbyEIDPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AJM_MPD_getResidentialGrantStatusbyEIDPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AJM_MPD_getResidentialGrantStatusbyEIDPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AJM_MPD_getResidentialGrantStatusbyEIDPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Output MOCDIntegrations.AJMMUNService.AJM_MPD_getResidentialGrantStatusbyEIDPortType.GetResidentialGrantStatus(MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Input request) {
            return base.Channel.GetResidentialGrantStatus(request);
        }
        
        public MOCDIntegrations.AJMMUNService.GetResidentialGrantStatusResponse GetResidentialGrantStatus(MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus GetResidentialGrantStatus1) {
            MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Input inValue = new MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Input();
            inValue.GetResidentialGrantStatus = GetResidentialGrantStatus1;
            MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Output retVal = ((MOCDIntegrations.AJMMUNService.AJM_MPD_getResidentialGrantStatusbyEIDPortType)(this)).GetResidentialGrantStatus(inValue);
            return retVal.GetResidentialGrantStatusResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Output> MOCDIntegrations.AJMMUNService.AJM_MPD_getResidentialGrantStatusbyEIDPortType.GetResidentialGrantStatusAsync(MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Input request) {
            return base.Channel.GetResidentialGrantStatusAsync(request);
        }
        
        public System.Threading.Tasks.Task<MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Output> GetResidentialGrantStatusAsync(MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus GetResidentialGrantStatus) {
            MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Input inValue = new MOCDIntegrations.AJMMUNService.GetResidentialGrantStatus_Input();
            inValue.GetResidentialGrantStatus = GetResidentialGrantStatus;
            return ((MOCDIntegrations.AJMMUNService.AJM_MPD_getResidentialGrantStatusbyEIDPortType)(this)).GetResidentialGrantStatusAsync(inValue);
        }
    }
}
