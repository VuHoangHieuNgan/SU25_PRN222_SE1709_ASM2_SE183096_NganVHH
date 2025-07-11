﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrugPrevention.Repositories.NganVHH.Models;

[Table("UserCoursesTuyenTM")]
public partial class UserCoursesTuyenTM
{
    [Key]
    public int UserCourseTuyenTMID { get; set; }

    public int UserID { get; set; }

    public int CourseID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EnrollmentDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CompletionDate { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Progress { get; set; }

    public int? Score { get; set; }

    public bool? CertificateIssued { get; set; }

    public int? Rating { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastAccessDate { get; set; }

    [ForeignKey("CourseID")]
    [InverseProperty("UserCoursesTuyenTMs")]
    public virtual CoursesQuangTNV Course { get; set; }

    [ForeignKey("UserID")]
    [InverseProperty("UserCoursesTuyenTMs")]
    public virtual UsersTuyenTM User { get; set; }
}