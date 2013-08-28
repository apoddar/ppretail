using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace PPRetail
{
    public class RetailManager
    {
        //This fetches initialization from PP
        //force upgradae possible
        //some config can be cached
        //singleton without locking

        static RetailManager _instance;
        public static RetailManager GetInstance() 
        {
            if(_instance != null)
                return _instance;
            else {
                _instance = new RetailManager();
            }
            return _instance;
        }
        public void InitializeConfig() 
        {
        }
        public ILocationManager GetLocationManager()
        {
            return new LocationManagerImpl();
        }
        public ITabManager GetTabManager()
        {
            return new TabManagerImpl();
        }
        
    }
    public interface ILocationManager
    {
        Location CreateLocation();
        Location OpenLocation(String locationId);
        Location CloseLocation(String locationId);
        Location ModifyLocation(Location location);
        Location AddLogo(Byte[] buffer);
        Location DeleteLogo(String locationId);

    }
    public class  LocationManagerImpl : ILocationManager
    {
        public Location CreateLocation() { return null; }
        public Location OpenLocation(String locationId) { return null; }
        public Location CloseLocation(String locationId) { return null; }
        public Location ModifyLocation(Location location) { return null; }
        public Location AddLogo(Byte[] buffer) { return null; }
        public Location DeleteLogo(String locationId) { return null; }
    }
    public interface ITabManager
    {
        Tab GetTab(String tabId);
        String AddInvoice(String tabId);
        Boolean RemoveInvoice(String tabId);
        TabPaymentResponse ChargeTab(Tab tab);
    }
    public class TabManagerImpl : ITabManager
    {
        public Tab GetTab(String tabId) 
        { return null; }
        public String AddInvoice(String tabId)
        { return null; }
        public Boolean RemoveInvoice(String tabId)
        { return false; }
        public TabPaymentResponse ChargeTab(Tab tab)
        { return null; }
    }
    public class TabPaymentResponse
    {
        public String txnId { get; set; }
    }
    public class Merchant
    {
        Merchant(String _status)
        {
            status = status;
        }
        String status { get; set; }
    }
    public class Location
    {
        public Location(String _id)
        {
             id = _id;
        }
        String name { get; set; }
        String internalName { get; set; }
        String id { get; set; }

        BigInteger latitude { get; set; }
        BigInteger longitude { get; set; }

        String availability { get; set; }
        BigInteger tabDuration { get; set; }
        String mobility { get; set; }

        String createDate { get; set; }
        String updateDate { get; set; }

        String status { get; set; }
    }
    public class LocationLogo
    {
        String url { get; set; }
    }
    public class Invoice
    {
        public Invoice (String _id)
        {
            id = _id;
        }
        String id {get ; set;}
        String merchantEmail { get; set; }
        InvoiceMerchantInfo merchantInfo { get; set; }

    }
    public class InvoiceMerchantInfo
    {
        String businessName { get; set; }
    }
    public class InvoiceItem
    {
    }
    public class Tab
    {
        public String id { set; get; }
        public String status { set; get; }
        public String customerName { set; get; }
        public String photoUrl { set; get; }

        public String createDate { set; get; }
        public String updateDate { set; get; }
        public String expirationDate { set; get; }

        public String invoiceId { set; get; }
        public String customerId { set; get; }
    }
    public class Payment
    {
        String tabId { set; get; }
        String invoiceId { set; get; }
        String paymentType { set; get; }
    }
    public class PPError
    {
        public BigInteger code { get; set; }
        public String message { get; set; }
        public String developerMessage { get; set; }
        public String type  { get; set; }
        public String correlationId { get; set; }

    }

}
