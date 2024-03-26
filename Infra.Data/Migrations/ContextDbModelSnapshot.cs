﻿// <auto-generated />
using System;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infra.Data.Migrations
{
    [DbContext(typeof(ContextDb))]
    partial class ContextDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Dominio.Contrato", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Empresa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EnderecoId")
                        .HasColumnType("int");

                    b.Property<string>("RazaoSocia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Responsavel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EnderecoId");

                    b.ToTable("Contratos");
                });

            modelBuilder.Entity("Domain.Dominio.Endereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("bairro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("complemento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ddd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("gia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ibge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("localidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("logradouro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("siafi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("uf")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("Domain.Dominio.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Domain.Dominio.TriboEquipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("TribosEquipes");
                });

            modelBuilder.Entity("Domain.Dominio.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContratoId")
                        .HasColumnType("int");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHashSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Tentativas")
                        .HasColumnType("int");

                    b.Property<int>("TriboEquipeId")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContratoId");

                    b.HasIndex("RoleId");

                    b.HasIndex("TriboEquipeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Dominio.menus.GroupMenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("GruposId")
                        .HasColumnType("int");

                    b.Property<int?>("MenusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GruposId");

                    b.HasIndex("MenusId");

                    b.ToTable("GroupMenu");
                });

            modelBuilder.Entity("Domain.Dominio.menus.Grupos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DataModificacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DtCriacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Grupo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdUsuarioCriacao")
                        .HasColumnType("int");

                    b.Property<string>("JustificativaModificacao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeUsuarioCriacao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Grupos");
                });

            modelBuilder.Entity("Domain.Dominio.menus.Menus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Route")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("Domain.Dominio.menus.RoleGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("GruposId")
                        .HasColumnType("int");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GruposId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolesGroups");
                });

            modelBuilder.Entity("Domain.Dominio.menus.Submenus.MenuSubmenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("MenusId")
                        .HasColumnType("int");

                    b.Property<int?>("SubmenuId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MenusId");

                    b.HasIndex("SubmenuId");

                    b.ToTable("MenusSubmenus");
                });

            modelBuilder.Entity("Domain.Dominio.menus.Submenus.Submenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Route")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Submenus");
                });

            modelBuilder.Entity("Domain.Dominio.Contrato", b =>
                {
                    b.HasOne("Domain.Dominio.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("EnderecoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Endereco");
                });

            modelBuilder.Entity("Domain.Dominio.Usuario", b =>
                {
                    b.HasOne("Domain.Dominio.Contrato", "Contrato")
                        .WithMany()
                        .HasForeignKey("ContratoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Dominio.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Dominio.TriboEquipe", "TriboEquipe")
                        .WithMany()
                        .HasForeignKey("TriboEquipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contrato");

                    b.Navigation("Role");

                    b.Navigation("TriboEquipe");
                });

            modelBuilder.Entity("Domain.Dominio.menus.GroupMenu", b =>
                {
                    b.HasOne("Domain.Dominio.menus.Grupos", null)
                        .WithMany("GroupMenus")
                        .HasForeignKey("GruposId");

                    b.HasOne("Domain.Dominio.menus.Menus", null)
                        .WithMany("MenuGroups")
                        .HasForeignKey("MenusId");
                });

            modelBuilder.Entity("Domain.Dominio.menus.RoleGroup", b =>
                {
                    b.HasOne("Domain.Dominio.menus.Grupos", null)
                        .WithMany("GroupRoles")
                        .HasForeignKey("GruposId");

                    b.HasOne("Domain.Dominio.Role", null)
                        .WithMany("RoleGroups")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Domain.Dominio.menus.Submenus.MenuSubmenu", b =>
                {
                    b.HasOne("Domain.Dominio.menus.Menus", null)
                        .WithMany("SubmenusMenu")
                        .HasForeignKey("MenusId");

                    b.HasOne("Domain.Dominio.menus.Submenus.Submenu", null)
                        .WithMany("MenuSubmenus")
                        .HasForeignKey("SubmenuId");
                });

            modelBuilder.Entity("Domain.Dominio.Role", b =>
                {
                    b.Navigation("RoleGroups");
                });

            modelBuilder.Entity("Domain.Dominio.menus.Grupos", b =>
                {
                    b.Navigation("GroupMenus");

                    b.Navigation("GroupRoles");
                });

            modelBuilder.Entity("Domain.Dominio.menus.Menus", b =>
                {
                    b.Navigation("MenuGroups");

                    b.Navigation("SubmenusMenu");
                });

            modelBuilder.Entity("Domain.Dominio.menus.Submenus.Submenu", b =>
                {
                    b.Navigation("MenuSubmenus");
                });
#pragma warning restore 612, 618
        }
    }
}
