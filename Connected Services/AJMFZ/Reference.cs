﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MOCDIntegrations.AJMFZ {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.informatica.com/dis/ws/", ConfigurationName="AJMFZ.AJM_DED_FZ_getLicenseDetailsbyPPPortType")]
    public interface AJM_DED_FZ_getLicenseDetailsbyPPPortType {
        
        // CODEGEN: Generating message contract since the operation GetLicenseDetails is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MOCDIntegrations.AJMFZ.GetLicenseDetails_Output GetLicenseDetails(MOCDIntegrations.AJMFZ.GetLicenseDetails_Input request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<MOCDIntegrations.AJMFZ.GetLicenseDetails_Output> GetLicenseDetailsAsync(MOCDIntegrations.AJMFZ.GetLicenseDetails_Input request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.informatica.com/dis/ws/")]
    public partial class GetLicenseDetails : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string passportNumberField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string PassportNumber {
            get {
                return this.passportNumberField;
            }
            set {
                this.passportNumberField = value;
                this.RaisePropertyChanged("PassportNumber");
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
    public partial class GetLicenseDetailsResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private GetLicenseDetailsResponseLicenseDetails[] licenseDetailsField;
        
        private GetLicenseDetailsResponseError errorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LicenseDetails", Order=0)]
        public GetLicenseDetailsResponseLicenseDetails[] LicenseDetails {
            get {
                return this.licenseDetailsField;
            }
            set {
                this.licenseDetailsField = value;
                this.RaisePropertyChanged("LicenseDetails");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public GetLicenseDetailsResponseError Error {
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
    public partial class GetLicenseDetailsResponseLicenseDetails : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string licenseNumberField;
        
        private string shareholderFirstNameField;
        
        private string shareholderLastNameField;
        
        private string shareHolderNationalityField;
        
        private string companyNameENField;
        
        private string companyNameARField;
        
        private string companyStatusField;
        
        private string licenseStartDateField;
        
        private string licenseExpiryDateField;
        
        private string shareHolderPercentageField;
        
        private string shareHolderNoOfSharesField;
        
        private string residenceVisaQuotaUsedField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string LicenseNumber {
            get {
                return this.licenseNumberField;
            }
            set {
                this.licenseNumberField = value;
                this.RaisePropertyChanged("LicenseNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ShareholderFirstName {
            get {
                return this.shareholderFirstNameField;
            }
            set {
                this.shareholderFirstNameField = value;
                this.RaisePropertyChanged("ShareholderFirstName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ShareholderLastName {
            get {
                return this.shareholderLastNameField;
            }
            set {
                this.shareholderLastNameField = value;
                this.RaisePropertyChanged("ShareholderLastName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string ShareHolderNationality {
            get {
                return this.shareHolderNationalityField;
            }
            set {
                this.shareHolderNationalityField = value;
                this.RaisePropertyChanged("ShareHolderNationality");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string CompanyNameEN {
            get {
                return this.companyNameENField;
            }
            set {
                this.companyNameENField = value;
                this.RaisePropertyChanged("CompanyNameEN");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string CompanyNameAR {
            get {
                return this.companyNameARField;
            }
            set {
                this.companyNameARField = value;
                this.RaisePropertyChanged("CompanyNameAR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string CompanyStatus {
            get {
                return this.companyStatusField;
            }
            set {
                this.companyStatusField = value;
                this.RaisePropertyChanged("CompanyStatus");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string LicenseStartDate {
            get {
                return this.licenseStartDateField;
            }
            set {
                this.licenseStartDateField = value;
                this.RaisePropertyChanged("LicenseStartDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string LicenseExpiryDate {
            get {
                return this.licenseExpiryDateField;
            }
            set {
                this.licenseExpiryDateField = value;
                this.RaisePropertyChanged("LicenseExpiryDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string ShareHolderPercentage {
            get {
                return this.shareHolderPercentageField;
            }
            set {
                this.shareHolderPercentageField = value;
                this.RaisePropertyChanged("ShareHolderPercentage");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string ShareHolderNoOfShares {
            get {
                return this.shareHolderNoOfSharesField;
            }
            set {
                this.shareHolderNoOfSharesField = value;
                this.RaisePropertyChanged("ShareHolderNoOfShares");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string ResidenceVisaQuotaUsed {
            get {
                return this.residenceVisaQuotaUsedField;
            }
            set {
                this.residenceVisaQuotaUsedField = value;
                this.RaisePropertyChanged("ResidenceVisaQuotaUsed");
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
    public partial class GetLicenseDetailsResponseError : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string inputParameterField;
        
        private string errorCodeField;
        
        private string errorDescriptionField;
        
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
        public string ErrorDescription {
            get {
                return this.errorDescriptionField;
            }
            set {
                this.errorDescriptionField = value;
                this.RaisePropertyChanged("ErrorDescription");
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
    public partial class GetLicenseDetails_Input {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.informatica.com/dis/ws/", Order=0)]
        public MOCDIntegrations.AJMFZ.GetLicenseDetails GetLicenseDetails;
        
        public GetLicenseDetails_Input() {
        }
        
        public GetLicenseDetails_Input(MOCDIntegrations.AJMFZ.GetLicenseDetails GetLicenseDetails) {
            this.GetLicenseDetails = GetLicenseDetails;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetLicenseDetails_Output {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.informatica.com/dis/ws/", Order=0)]
        public MOCDIntegrations.AJMFZ.GetLicenseDetailsResponse GetLicenseDetailsResponse;
        
        public GetLicenseDetails_Output() {
        }
        
        public GetLicenseDetails_Output(MOCDIntegrations.AJMFZ.GetLicenseDetailsResponse GetLicenseDetailsResponse) {
            this.GetLicenseDetailsResponse = GetLicenseDetailsResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AJM_DED_FZ_getLicenseDetailsbyPPPortTypeChannel : MOCDIntegrations.AJMFZ.AJM_DED_FZ_getLicenseDetailsbyPPPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AJM_DED_FZ_getLicenseDetailsbyPPPortTypeClient : System.ServiceModel.ClientBase<MOCDIntegrations.AJMFZ.AJM_DED_FZ_getLicenseDetailsbyPPPortType>, MOCDIntegrations.AJMFZ.AJM_DED_FZ_getLicenseDetailsbyPPPortType {
        
        public AJM_DED_FZ_getLicenseDetailsbyPPPortTypeClient() {
        }
        
        public AJM_DED_FZ_getLicenseDetailsbyPPPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AJM_DED_FZ_getLicenseDetailsbyPPPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AJM_DED_FZ_getLicenseDetailsbyPPPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AJM_DED_FZ_getLicenseDetailsbyPPPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MOCDIntegrations.AJMFZ.GetLicenseDetails_Output MOCDIntegrations.AJMFZ.AJM_DED_FZ_getLicenseDetailsbyPPPortType.GetLicenseDetails(MOCDIntegrations.AJMFZ.GetLicenseDetails_Input request) {
            return base.Channel.GetLicenseDetails(request);
        }
        
        public MOCDIntegrations.AJMFZ.GetLicenseDetailsResponse GetLicenseDetails(MOCDIntegrations.AJMFZ.GetLicenseDetails GetLicenseDetails1) {
            MOCDIntegrations.AJMFZ.GetLicenseDetails_Input inValue = new MOCDIntegrations.AJMFZ.GetLicenseDetails_Input();
            inValue.GetLicenseDetails = GetLicenseDetails1;
            MOCDIntegrations.AJMFZ.GetLicenseDetails_Output retVal = ((MOCDIntegrations.AJMFZ.AJM_DED_FZ_getLicenseDetailsbyPPPortType)(this)).GetLicenseDetails(inValue);
            return retVal.GetLicenseDetailsResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MOCDIntegrations.AJMFZ.GetLicenseDetails_Output> MOCDIntegrations.AJMFZ.AJM_DED_FZ_getLicenseDetailsbyPPPortType.GetLicenseDetailsAsync(MOCDIntegrations.AJMFZ.GetLicenseDetails_Input request) {
            return base.Channel.GetLicenseDetailsAsync(request);
        }
        
        public System.Threading.Tasks.Task<MOCDIntegrations.AJMFZ.GetLicenseDetails_Output> GetLicenseDetailsAsync(MOCDIntegrations.AJMFZ.GetLicenseDetails GetLicenseDetails) {
            MOCDIntegrations.AJMFZ.GetLicenseDetails_Input inValue = new MOCDIntegrations.AJMFZ.GetLicenseDetails_Input();
            inValue.GetLicenseDetails = GetLicenseDetails;
            return ((MOCDIntegrations.AJMFZ.AJM_DED_FZ_getLicenseDetailsbyPPPortType)(this)).GetLicenseDetailsAsync(inValue);
        }
    }
}
