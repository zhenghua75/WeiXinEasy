﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mozart.srChargeFee {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="wcRequestData", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class wcRequestData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OpenIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OrderIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ChargeNoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ChargeTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ChargeAmountField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string OpenID {
            get {
                return this.OpenIDField;
            }
            set {
                if ((object.ReferenceEquals(this.OpenIDField, value) != true)) {
                    this.OpenIDField = value;
                    this.RaisePropertyChanged("OpenID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string OrderID {
            get {
                return this.OrderIDField;
            }
            set {
                if ((object.ReferenceEquals(this.OrderIDField, value) != true)) {
                    this.OrderIDField = value;
                    this.RaisePropertyChanged("OrderID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string ChargeNo {
            get {
                return this.ChargeNoField;
            }
            set {
                if ((object.ReferenceEquals(this.ChargeNoField, value) != true)) {
                    this.ChargeNoField = value;
                    this.RaisePropertyChanged("ChargeNo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string ChargeType {
            get {
                return this.ChargeTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.ChargeTypeField, value) != true)) {
                    this.ChargeTypeField = value;
                    this.RaisePropertyChanged("ChargeType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string ChargeAmount {
            get {
                return this.ChargeAmountField;
            }
            set {
                if ((object.ReferenceEquals(this.ChargeAmountField, value) != true)) {
                    this.ChargeAmountField = value;
                    this.RaisePropertyChanged("ChargeAmount");
                }
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="wcResponseData", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class wcResponseData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RetField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MsgField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string Ret {
            get {
                return this.RetField;
            }
            set {
                if ((object.ReferenceEquals(this.RetField, value) != true)) {
                    this.RetField = value;
                    this.RaisePropertyChanged("Ret");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Msg {
            get {
                return this.MsgField;
            }
            set {
                if ((object.ReferenceEquals(this.MsgField, value) != true)) {
                    this.MsgField = value;
                    this.RaisePropertyChanged("Msg");
                }
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="srChargeFee.OtherServiceSoap")]
    public interface OtherServiceSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 modelWXCharge 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PutChargeFee", ReplyAction="*")]
        Mozart.srChargeFee.PutChargeFeeResponse PutChargeFee(Mozart.srChargeFee.PutChargeFeeRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PutChargeFeeRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PutChargeFee", Namespace="http://tempuri.org/", Order=0)]
        public Mozart.srChargeFee.PutChargeFeeRequestBody Body;
        
        public PutChargeFeeRequest() {
        }
        
        public PutChargeFeeRequest(Mozart.srChargeFee.PutChargeFeeRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PutChargeFeeRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public Mozart.srChargeFee.wcRequestData modelWXCharge;
        
        public PutChargeFeeRequestBody() {
        }
        
        public PutChargeFeeRequestBody(Mozart.srChargeFee.wcRequestData modelWXCharge) {
            this.modelWXCharge = modelWXCharge;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PutChargeFeeResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PutChargeFeeResponse", Namespace="http://tempuri.org/", Order=0)]
        public Mozart.srChargeFee.PutChargeFeeResponseBody Body;
        
        public PutChargeFeeResponse() {
        }
        
        public PutChargeFeeResponse(Mozart.srChargeFee.PutChargeFeeResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PutChargeFeeResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public Mozart.srChargeFee.wcResponseData PutChargeFeeResult;
        
        public PutChargeFeeResponseBody() {
        }
        
        public PutChargeFeeResponseBody(Mozart.srChargeFee.wcResponseData PutChargeFeeResult) {
            this.PutChargeFeeResult = PutChargeFeeResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface OtherServiceSoapChannel : Mozart.srChargeFee.OtherServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OtherServiceSoapClient : System.ServiceModel.ClientBase<Mozart.srChargeFee.OtherServiceSoap>, Mozart.srChargeFee.OtherServiceSoap {
        
        public OtherServiceSoapClient() {
        }
        
        public OtherServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OtherServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OtherServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OtherServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Mozart.srChargeFee.PutChargeFeeResponse Mozart.srChargeFee.OtherServiceSoap.PutChargeFee(Mozart.srChargeFee.PutChargeFeeRequest request) {
            return base.Channel.PutChargeFee(request);
        }
        
        public Mozart.srChargeFee.wcResponseData PutChargeFee(Mozart.srChargeFee.wcRequestData modelWXCharge) {
            Mozart.srChargeFee.PutChargeFeeRequest inValue = new Mozart.srChargeFee.PutChargeFeeRequest();
            inValue.Body = new Mozart.srChargeFee.PutChargeFeeRequestBody();
            inValue.Body.modelWXCharge = modelWXCharge;
            Mozart.srChargeFee.PutChargeFeeResponse retVal = ((Mozart.srChargeFee.OtherServiceSoap)(this)).PutChargeFee(inValue);
            return retVal.Body.PutChargeFeeResult;
        }
    }
}
