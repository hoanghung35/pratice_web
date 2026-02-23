using Content_App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Content_App.Infrastructure.Data;

public partial class LogDbContext : DbContext
{
    public LogDbContext()
    {
    }

    public LogDbContext(DbContextOptions<LogDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Approve> Approves { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<LogAction> LogActions { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=LOG_CONTROL;Username=postgres;Password=tslog");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_pkey");

            entity.ToTable("account");

            entity.HasIndex(e => e.UserCode, "account_user_code_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UserCode)
                .HasColumnType("character varying")
                .HasColumnName("user_code");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_account_role");
        });

        modelBuilder.Entity<Approve>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("approve_pkey");

            entity.ToTable("approve");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.DateApprove)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_approve");
            entity.Property(e => e.DateRequest)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_request");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Kind)
                .HasColumnType("character varying")
                .HasColumnName("kind");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.RequestorId).HasColumnName("requestor_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.Item).WithMany(p => p.Approves)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_approve_item");

            entity.HasOne(d => d.Order).WithMany(p => p.Approves)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_approve_order");

            entity.HasOne(d => d.Requestor).WithMany(p => p.Approves)
                .HasForeignKey(d => d.RequestorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_approve_requestor");
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("area_pkey");

            entity.ToTable("area");

            entity.HasIndex(e => e.AreaCode, "area_area_code_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.AreaCode)
                .HasColumnType("character varying")
                .HasColumnName("area_code");
            entity.Property(e => e.AreaName)
                .HasColumnType("character varying")
                .HasColumnName("area_name");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("currency_pkey");

            entity.ToTable("currency");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CurrenName)
                .HasColumnType("character varying")
                .HasColumnName("curren_name");
            entity.Property(e => e.ExchangeRate)
                .HasPrecision(15, 3)
                .HasColumnName("exchange_rate");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("department_pkey");

            entity.ToTable("department");

            entity.HasIndex(e => e.DeptCode, "department_dept_code_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.DeptCode)
                .HasColumnType("character varying")
                .HasColumnName("dept_code");
            entity.Property(e => e.DeptName)
                .HasColumnType("character varying")
                .HasColumnName("dept_name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.HasIndex(e => e.EmpCode, "employee_emp_code_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.EmpCode)
                .HasColumnType("character varying")
                .HasColumnName("emp_code");
            entity.Property(e => e.FullName)
                .HasColumnType("character varying")
                .HasColumnName("fullname");
            entity.Property(e => e.Grade)
                .HasColumnType("character varying")
                .HasColumnName("grade");

            entity.HasOne(d => d.Area).WithMany(p => p.Employees)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_emp_area");

            entity.HasOne(d => d.Dept).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DeptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_emp_dept");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("item_pkey");

            entity.ToTable("item");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.Cost)
                .HasPrecision(15, 3)
                .HasColumnName("cost");
            entity.Property(e => e.Currency)
                .HasColumnType("character varying")
                .HasColumnName("currency");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.EnName)
                .HasColumnType("character varying")
                .HasColumnName("en_name");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.ItemCode)
                .HasColumnType("character varying")
                .HasColumnName("item_code");
            entity.Property(e => e.Maker)
                .HasColumnType("character varying")
                .HasColumnName("maker");
            entity.Property(e => e.PositionIn)
                .HasColumnType("character varying")
                .HasColumnName("position_in");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");
            entity.Property(e => e.Supplier)
                .HasColumnType("character varying")
                .HasColumnName("supplier");
            entity.Property(e => e.Unit)
                .HasColumnType("character varying")
                .HasColumnName("unit");
            entity.Property(e => e.VnName)
                .HasColumnType("character varying")
                .HasColumnName("vn_name");

            entity.HasOne(d => d.Area).WithMany(p => p.Items)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_area");

            entity.HasOne(d => d.Dept).WithMany(p => p.Items)
                .HasForeignKey(d => d.DeptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_dep");
        });

        modelBuilder.Entity<LogAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("action_pkey");

            entity.ToTable("log_action");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.DateAction)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_action");
            entity.Property(e => e.EmpCode)
                .HasMaxLength(10)
                .HasColumnName("emp_code");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Kind)
                .HasColumnType("character varying")
                .HasColumnName("kind");
            entity.Property(e => e.PicId).HasColumnName("pic_id");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Reason).HasColumnName("reason");

            entity.HasOne(d => d.Item).WithMany(p => p.LogActions)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_action_item");

            entity.HasOne(d => d.Pic).WithMany(p => p.LogActions)
                .HasForeignKey(d => d.PicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_action_pic");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_item_pkey");

            entity.ToTable("order_item");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.DateOrder)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_order");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.PicId).HasColumnName("pic_id");
            entity.Property(e => e.PlanOrder).HasColumnName("plan_order");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Reason).HasColumnName("reason");

            entity.HasOne(d => d.Item).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_item");

            entity.HasOne(d => d.Pic).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.PicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_pic");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.RoleName, "role_role_num_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Descreption)
                .HasColumnType("character varying")
                .HasColumnName("descreption");
            entity.Property(e => e.RoleName)
                .HasColumnType("character varying")
                .HasColumnName("role_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
