﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DSS.MoHra.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionVariant> QuestionVariants { get; set; }
        public virtual DbSet<ResolverCondition> ResolverConditions { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<ResultGroup> ResultGroups { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSession> UserSessions { get; set; }
        public virtual DbSet<UserInformation> UserInformations { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
    }
}
