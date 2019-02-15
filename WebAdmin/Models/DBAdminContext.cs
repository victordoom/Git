using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebAdmin.Models
{
    public  class DBAdminContext : DbContext
    {
        public DBAdminContext()
        {

        }
        
        public DBAdminContext(DbContextOptions<DBAdminContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cases> Cases { get; set; }
        public virtual DbSet<CasesDetails> CasesDetails { get; set; }
        public virtual DbSet<CasesCategories> CasesCategories { get; set; }
        public virtual DbSet<SalesCompany> SalesCompany { get; set; }
        public virtual DbSet<SalesLocations> SalesLocations { get; set; }
        public virtual DbSet<SegUsuarios> SegUsuarios { get; set; }
        public virtual DbSet<CasesPriority> CasesPriority { get; set; }
        public virtual DbSet<CasesStatus> CasesStatus { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Vehicles> Vehicles { get; set; }
        public virtual DbSet<Opportunities> Opportunities { get; set; }
        public virtual DbSet<OpportunitiesHowFound> OpportunitiesHowFound { get; set; }
        public virtual DbSet<OpportunitiesCategories> OpportunitiesCategories { get; set; }
        public virtual DbSet<Programs> Programs { get; set; }
        public virtual DbSet<OpportunitiesDetails> OpportunitiesDetails { get; set; }
        public DbQuery<Techfiles> Techfiles { get; set; }

        public virtual DbSet<SegSistemaUsuario> SegSistemaUsuario { get; set; }
        public virtual DbSet<Sistema> Sistemas { get; set; }
        public virtual DbSet<Opciones> Opciones { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<SalesComments> SalesComments { get; set; }



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //        optionsBuilder.UseSqlServer("Server=96.231.33.87;Database=DBAdmin;MultipleActiveResultSets=true");

        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            //modelBuilder.Query<vwtechfiles>().ToView("vw_techFiles");
            //.Property(v => v.ContractID).HasColumnName("ContractID");

            modelBuilder.Entity<Cases>(entity =>
            {
                entity.Property(e => e.CasesID).HasColumnName("CasesID");

                entity.Property(e => e.AssignedToSendEmail)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Attachments)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CallerEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CallerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CallerPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CallerTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Comments)
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.ContractId)
                    .HasColumnName("ContractID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.IssueTypeId).HasColumnName("IssueTypeID");

                entity.Property(e => e.Kb).HasColumnName("KB");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.OpenedDate).HasColumnType("datetime");

                entity.Property(e => e.Priority)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RelatedCases)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ResolvedDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.LastComment)
                 .HasComputedColumnSql("[dbo].[Cases_GetLastUpdate]([ID])");

          

                entity.Property(e => e.TypeId)
                    .HasColumnName("TypeID")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                .HasColumnName("FileName")
                .HasMaxLength(250)
                .IsUnicode(false);

                entity.Property(e => e.FileType)
                .HasColumnName("FileType")
                .HasMaxLength(50)
                .IsUnicode(false);

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.AssignedTo)
                    .HasConstraintName("FK_Cases_SEG_USUARIOS");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Cases_SalesCompany");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Cases_SalesLocations");
            });

            modelBuilder.Entity<CasesDetails>(entity =>
            {
                entity.HasKey(e => e.DetailId);

                entity.Property(e => e.DetailId).HasColumnName("DetailID");

                entity.Property(e => e.CasesID).HasColumnName("CasesID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.DetailDatetime).HasColumnType("datetime");

                entity.Property(e => e.NormalRow)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Status)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Suggestion)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.UserID).HasColumnName("UserID");

                entity.HasOne(d => d.Cases)
                    .WithMany(p => p.CasesDetails)
                    .HasForeignKey(d => d.CasesID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CasesDetails_Cases");

                entity.HasOne(d => d.SegUsuarios)
                    .WithMany(p => p.CasesDetails)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK_CasesDetails_SEG_USUARIOS");
            });

            modelBuilder.Entity<CasesCategories>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IncomeExpense)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<CasesPriority>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

            }); modelBuilder.Entity<CasesStatus>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

            });


            modelBuilder.Entity<Opportunities>(entity =>
            {
                entity.Property(e => e.LastComment)
              .HasComputedColumnSql("[dbo].[Cases_GetLastUpdate]([ID])");
            });


            modelBuilder.Entity<SalesCompany>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            });

            modelBuilder.Entity<SalesLocations>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.ContactBirthday)
                    .HasColumnName("Contact_Birthday")
                    .HasColumnType("datetime");

                entity.Property(e => e.ContactName)
                    .HasColumnName("Contact_Name")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ContactOwnerId).HasColumnName("Contact_OwnerID");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DbaAddress)
                    .HasColumnName("DBA_Address")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DbaCity)
                    .HasColumnName("DBA_City")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DbaName)
                    .HasColumnName("DBA_Name")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DbaState)
                    .HasColumnName("DBA_State")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DbaZipCode)
                    .HasColumnName("DBA_ZipCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EmailMerchant)
                    .HasColumnName("Email_Merchant")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HomePhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LegalName)
                    .HasColumnName("Legal_Name")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.MobilePhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TaxId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WebPage)
                    .HasColumnName("webPage")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.SalesLocations)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesLocations_SalesCompany");
            });

            modelBuilder.Entity<SegUsuarios>(entity =>
            {
                entity.HasKey(e => e.UserID)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("Seg_Usuarios");

                entity.Property(e => e.UserID).HasColumnName("UserID");

                entity.Property(e => e.Acceso)
                    .HasColumnName("ACCESO")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.ApellidoUsuario)
                    .HasColumnName("APELLIDO_USUARIO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cargo)
                    .HasColumnName("CARGO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ccodana)
                    .HasColumnName("ccodana")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.CodEmp)
                    .HasColumnName("COD_EMP")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.DireccionUsuario)
                    .HasColumnName("DIRECCION_USUARIO")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DmUser)
                    .HasColumnName("DM_USER")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dui)
                    .HasColumnName("DUI")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoUsuario)
                    .HasColumnName("ESTADO_USUARIO")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnName("FECHA_REGISTRO")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaUltimoLogeo)
                    .HasColumnName("FECHA_ULTIMO_LOGEO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Gestor)
                    .HasColumnName("GESTOR")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.IdPersona)
                    .HasColumnName("ID_PERSONA")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ImagenFirma)
                    .HasColumnName("IMAGEN_FIRMA")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .HasColumnName("LOGIN")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreUsuario)
                    .HasColumnName("NOMBRE_USUARIO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WebPassword)
                    .HasColumnName("WebPassword")
                    .IsUnicode(false);

               entity.Property(e => e.Sexo)
                    .HasColumnName("SEXO")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TelefonoUsuario)
                    .HasColumnName("TELEFONO_USUARIO")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VigenciaLogin)
                    .HasColumnName("VIGENCIA_LOGIN")
                    .HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<SegSistemaUsuario>(entity =>
            {
                entity.ToTable("SEG_SISTEMA_USUARIO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Alcance)
                    .HasColumnName("ALCANCE")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.CodigoPerfil)
                    .HasColumnName("CODIGO_PERFIL")
                    .HasColumnType("numeric(3, 0)");

                entity.Property(e => e.CodigoSistema)
                    .HasColumnName("CODIGO_SISTEMA")
                    .HasColumnType("numeric(3, 0)");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("ID_USUARIO")
                    .HasColumnType("numeric(5, 0)");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.SegSistemaUsuario)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SEG_SISTEMA_USUARIO_SEG_USUARIOS");
            });

            modelBuilder.Entity<Sistema>(entity =>
            {
                entity.HasKey(e => e.CodigoSistema);

                entity.ToTable("SISTEMA");

                entity.Property(e => e.CodigoSistema)
                    .HasColumnName("CODIGO_SISTEMA")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.Abreviatura)
                    .HasColumnName("ABREVIATURA")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Cdgmodulo)
                    .HasColumnName("CDGMODULO")
                    .HasColumnType("char(2)");

                entity.Property(e => e.CodigoSistemaAnt)
                    .HasColumnName("CODIGO_SISTEMA_ANT")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.DescripcionModulo)
                    .HasColumnName("DESCRIPCION_MODULO")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasColumnName("ESTADO")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaImplantacion)
                    .HasColumnName("FECHA_IMPLANTACION")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nivel1)
                    .HasColumnName("NIVEL1")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.Nivel2)
                    .HasColumnName("NIVEL2")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.NombreCorto)
                    .HasColumnName("NOMBRE_CORTO")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NombreSistema)
                    .HasColumnName("NOMBRE_SISTEMA")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ObjetivoModulo)
                    .HasColumnName("OBJETIVO_MODULO")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.RutaCarpetaFotos)
                    .HasColumnName("RUTA_CARPETA_FOTOS")
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Opciones>(entity =>
            {
                entity.HasKey(e => e.IdOpcion);

                entity.ToTable("OPCIONES");

                entity.Property(e => e.IdOpcion)
                    .HasColumnName("ID_OPCION")
                    .HasColumnType("numeric(5, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Central)
                    .HasColumnName("CENTRAL")
                    .HasColumnType("char(1)");

                entity.Property(e => e.CodigoSistema)
                    .HasColumnName("CODIGO_SISTEMA")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.Formulario)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Icono)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Nivel1)
                    .HasColumnName("NIVEL1")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.Nivel2)
                    .HasColumnName("NIVEL2")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.Nivel3)
                    .HasColumnName("NIVEL3")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.Nivel4)
                    .HasColumnName("NIVEL4")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.NombreOpcion)
                    .HasColumnName("NOMBRE_OPCION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodigoSistemaNavigation)
                    .WithMany(p => p.Opciones)
                    .HasForeignKey(d => d.CodigoSistema)
                    .HasConstraintName("FK_OPCIONES_SISTEMA_O_SISTEMA");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Accounting).HasColumnName("ACCOUNTING");

                entity.Property(e => e.ActivarShift).HasColumnName("Activar_Shift");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Administration).HasColumnName("ADMINISTRATION");

                entity.Property(e => e.Attachments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Birthdate).HasColumnType("datetime");

                entity.Property(e => e.BusinessPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Company)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CountryRegion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FaxNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HomePhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Inventory).HasColumnName("INVENTORY");

                entity.Property(e => e.JobTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobilePhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MostrarCintaOpciones).HasColumnName("Mostrar_Cinta_Opciones");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.OfficeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pass)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PositionId).HasColumnName("PositionID");

                entity.Property(e => e.Salesdepartament).HasColumnName("SALESDEPARTAMENT");

                entity.Property(e => e.Social)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StateProvince)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaxId)
                    .HasColumnName("TaxID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Technicalsupport).HasColumnName("TECHNICALSUPPORT");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.WebPage)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ZippostalCode)
                    .HasColumnName("ZIPPostalCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SalesComments>(entity =>
            {
                entity.HasKey(e => e.CommentId);

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.Property(e => e.CommentDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SalesId).HasColumnName("SalesID");

                entity.Property(e => e.Title)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });
        }
    }
}
