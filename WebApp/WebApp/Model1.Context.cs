﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class hospitalEntities : DbContext
    {
        public hospitalEntities()
            : base("name=hospitalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<admin> admins { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_address> tbl_address { get; set; }
        public virtual DbSet<tbl_appointment> tbl_appointment { get; set; }
        public virtual DbSet<tbl_contact> tbl_contact { get; set; }
        public virtual DbSet<tbl_doctors> tbl_doctors { get; set; }
        public virtual DbSet<tbl_gallery> tbl_gallery { get; set; }
        public virtual DbSet<tbl_hospitalDetail> tbl_hospitalDetail { get; set; }
        public virtual DbSet<tbl_queries> tbl_queries { get; set; }
        public virtual DbSet<tbl_replies> tbl_replies { get; set; }
        public virtual DbSet<tbl_services> tbl_services { get; set; }
        public virtual DbSet<tbl_slider> tbl_slider { get; set; }
        public virtual DbSet<tbl_subscriptions> tbl_subscriptions { get; set; }
        public virtual DbSet<tbl_users> tbl_users { get; set; }
    }
}
