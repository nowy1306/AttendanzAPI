using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AttendanzApi.Models;

namespace AttendanzApi.Db
{
    public class AppDbContext : DbContext
    {
        public DbSet<GroupModel> Groups { get; set; }
        public DbSet<ClassModel> Classes { get; set; }
        public DbSet<GroupStudentModel> GroupStudents { get; set; }
        public DbSet<PresenceModel> Presences { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<SubjectModel> Subjects { get; set; }
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<ScannerModel> Scanners { get; set; }
        public DbSet<ControlProcessModel> ControlProcesses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
