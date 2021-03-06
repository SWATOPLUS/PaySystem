﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PaySystem.Data;
using System;

namespace PaySystem.Data.Migrations
{
    [DbContext(typeof(ForeignDbContext))]
    partial class ForeignDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PaySystem.Models.BusinessModels.GlobalWorker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Biography");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Name");

                    b.Property<DateTime>("UpdateTimeDateTime");

                    b.HasKey("Id");

                    b.ToTable("Worker");
                });
#pragma warning restore 612, 618
        }
    }
}
