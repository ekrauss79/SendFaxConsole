//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SendFaxConsole.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblFaxRequestMaster
    {
        public int FaxRequestID { get; set; }
        public int ClientID { get; set; }
        public string Fax_File_Location { get; set; }
        public System.DateTime Date_Requested { get; set; }
        public Nullable<System.DateTime> Date_Last_Sent { get; set; }
        public int MessageID { get; set; }
        public string Message_Section1_Subject { get; set; }
        public string Message_Section2_Body1 { get; set; }
        public string Message_Section3_Body2 { get; set; }
        public string Message_Section4_Body3 { get; set; }
        public string Message_Section5_Body4 { get; set; }
        public string Message_Section6_Body5 { get; set; }
        public string Message_Section7_Body6 { get; set; }
        public string Message_Section8_Body7 { get; set; }
        public string Message_Section9_Body8 { get; set; }
    }
}
