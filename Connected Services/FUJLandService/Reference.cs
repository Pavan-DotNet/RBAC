﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MOCDIntegrations.FUJLandService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.informatica.com/dis/ws/", ConfigurationName="FUJLandService.FUJ_MUN_getLandDetailsbyEIDPortType")]
    public interface FUJ_MUN_getLandDetailsbyEIDPortType {
        
        // CODEGEN: Generating message contract since the operation GetLandDetails is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MOCDIntegrations.FUJLandService.GetLandDetails_Output GetLandDetails(MOCDIntegrations.FUJLandService.GetLandDetails_Input request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<MOCDIntegrations.FUJLandService.GetLandDetails_Output> GetLandDetailsAsync(MOCDIntegrations.FUJLandService.GetLandDetails_Input request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.informatica.com/dis/ws/")]
    public partial class GetLandDetails : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string emiratesIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string EmiratesID {
            get {
                return this.emiratesIDField;
            }
            set {
                this.emiratesIDField = value;
                this.RaisePropertyChanged("EmiratesID");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.informatica.com/dis/ws/")]
    public partial class GetLandDetailsResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private GetLandDetailsResponseLandDetails[] landDetailsField;
        
        private GetLandDetailsResponseError errorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LandDetails", Order=0)]
        public GetLandDetailsResponseLandDetails[] LandDetails {
            get {
                return this.landDetailsField;
            }
            set {
                this.landDetailsField = value;
                this.RaisePropertyChanged("LandDetails");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public GetLandDetailsResponseError Error {
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.informatica.com/dis/ws/")]
    public partial class GetLandDetailsResponseLandDetails : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string emiratesIDField;
        
        private string realEstateOwnerField;
        
        private string ownershipPercentageField;
        
        private string realStateLocationField;
        
        private string realEstateNumberField;
        
        private string ownershipDateField;
        
        private string realEstateTypeField;
        
        private string realEstateStatusField;
        
        private string plotNumberField;
        
        private string blockNumberField;
        
        private string lastRefreshDateField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string EmiratesID {
            get {
                return this.emiratesIDField;
            }
            set {
                this.emiratesIDField = value;
                this.RaisePropertyChanged("EmiratesID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string RealEstateOwner {
            get {
                return this.realEstateOwnerField;
            }
            set {
                this.realEstateOwnerField = value;
                this.RaisePropertyChanged("RealEstateOwner");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string OwnershipPercentage {
            get {
                return this.ownershipPercentageField;
            }
            set {
                this.ownershipPercentageField = value;
                this.RaisePropertyChanged("OwnershipPercentage");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string RealStateLocation {
            get {
                return this.realStateLocationField;
            }
            set {
                this.realStateLocationField = value;
                this.RaisePropertyChanged("RealStateLocation");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string RealEstateNumber {
            get {
                return this.realEstateNumberField;
            }
            set {
                this.realEstateNumberField = value;
                this.RaisePropertyChanged("RealEstateNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string OwnershipDate {
            get {
                return this.ownershipDateField;
            }
            set {
                this.ownershipDateField = value;
                this.RaisePropertyChanged("OwnershipDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string RealEstateType {
            get {
                return this.realEstateTypeField;
            }
            set {
                this.realEstateTypeField = value;
                this.RaisePropertyChanged("RealEstateType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string RealEstateStatus {
            get {
                return this.realEstateStatusField;
            }
            set {
                this.realEstateStatusField = value;
                this.RaisePropertyChanged("RealEstateStatus");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string PlotNumber {
            get {
                return this.plotNumberField;
            }
            set {
                this.plotNumberField = value;
                this.RaisePropertyChanged("PlotNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string BlockNumber {
            get {
                return this.blockNumberField;
            }
            set {
                this.blockNumberField = value;
                this.RaisePropertyChanged("BlockNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string LastRefreshDate {
            get {
                return this.lastRefreshDateField;
            }
            set {
                this.lastRefreshDateField = value;
                this.RaisePropertyChanged("LastRefreshDate");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.informatica.com/dis/ws/")]
    public partial class GetLandDetailsResponseError : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string errorDescriptionField;
        
        private string errorCodeField;
        
        private string inputParameterField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string ErrorDescription {
            get {
                return this.errorDescriptionField;
            }
            set {
                this.errorDescriptionField = value;
                this.RaisePropertyChanged("ErrorDescription");
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
        public string InputParameter {
            get {
                return this.inputParameterField;
            }
            set {
                this.inputParameterField = value;
                this.RaisePropertyChanged("InputParameter");
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
    public partial class GetLandDetails_Input {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.informatica.com/dis/ws/", Order=0)]
        public MOCDIntegrations.FUJLandService.GetLandDetails GetLandDetails;
        
        public GetLandDetails_Input() {
        }
        
        public GetLandDetails_Input(MOCDIntegrations.FUJLandService.GetLandDetails GetLandDetails) {
            this.GetLandDetails = GetLandDetails;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetLandDetails_Output {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.informatica.com/dis/ws/", Order=0)]
        public MOCDIntegrations.FUJLandService.GetLandDetailsResponse GetLandDetailsResponse;
        
        public GetLandDetails_Output() {
        }
        
        public GetLandDetails_Output(MOCDIntegrations.FUJLandService.GetLandDetailsResponse GetLandDetailsResponse) {
            this.GetLandDetailsResponse = GetLandDetailsResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface FUJ_MUN_getLandDetailsbyEIDPortTypeChannel : MOCDIntegrations.FUJLandService.FUJ_MUN_getLandDetailsbyEIDPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FUJ_MUN_getLandDetailsbyEIDPortTypeClient : System.ServiceModel.ClientBase<MOCDIntegrations.FUJLandService.FUJ_MUN_getLandDetailsbyEIDPortType>, MOCDIntegrations.FUJLandService.FUJ_MUN_getLandDetailsbyEIDPortType {
        
        public FUJ_MUN_getLandDetailsbyEIDPortTypeClient() {
        }
        
        public FUJ_MUN_getLandDetailsbyEIDPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FUJ_MUN_getLandDetailsbyEIDPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FUJ_MUN_getLandDetailsbyEIDPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FUJ_MUN_getLandDetailsbyEIDPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MOCDIntegrations.FUJLandService.GetLandDetails_Output MOCDIntegrations.FUJLandService.FUJ_MUN_getLandDetailsbyEIDPortType.GetLandDetails(MOCDIntegrations.FUJLandService.GetLandDetails_Input request) {
            return base.Channel.GetLandDetails(request);
        }
        
        public MOCDIntegrations.FUJLandService.GetLandDetailsResponse GetLandDetails(MOCDIntegrations.FUJLandService.GetLandDetails GetLandDetails1) {
            MOCDIntegrations.FUJLandService.GetLandDetails_Input inValue = new MOCDIntegrations.FUJLandService.GetLandDetails_Input();
            inValue.GetLandDetails = GetLandDetails1;
            MOCDIntegrations.FUJLandService.GetLandDetails_Output retVal = ((MOCDIntegrations.FUJLandService.FUJ_MUN_getLandDetailsbyEIDPortType)(this)).GetLandDetails(inValue);
            return retVal.GetLandDetailsResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MOCDIntegrations.FUJLandService.GetLandDetails_Output> MOCDIntegrations.FUJLandService.FUJ_MUN_getLandDetailsbyEIDPortType.GetLandDetailsAsync(MOCDIntegrations.FUJLandService.GetLandDetails_Input request) {
            return base.Channel.GetLandDetailsAsync(request);
        }
        
        public System.Threading.Tasks.Task<MOCDIntegrations.FUJLandService.GetLandDetails_Output> GetLandDetailsAsync(MOCDIntegrations.FUJLandService.GetLandDetails GetLandDetails) {
            MOCDIntegrations.FUJLandService.GetLandDetails_Input inValue = new MOCDIntegrations.FUJLandService.GetLandDetails_Input();
            inValue.GetLandDetails = GetLandDetails;
            return ((MOCDIntegrations.FUJLandService.FUJ_MUN_getLandDetailsbyEIDPortType)(this)).GetLandDetailsAsync(inValue);
        }
    }
}
