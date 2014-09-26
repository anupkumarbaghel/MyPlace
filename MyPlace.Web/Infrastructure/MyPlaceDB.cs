using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MyPlace.Domain;
using MyPlace.Domain.Shoping;
using MyPlace.Domain.NoteBook;

namespace MyPlace.Web.Infrastructure
{
    public class MyPlaceDB:DbContext
    {
       public DbSet<Home> Homes { get; set; }
       public DbSet<HomeDescription> HomeDescriptions { get; set; }
       public DbSet<Category> Category { get; set; }
       public DbSet<Address> Address { get; set; }
       public DbSet<Product> Product { get; set; }
       public DbSet<ProductCategory> ProductCategory { get; set; }
       public DbSet<Feature> Feature { get; set; }
       public DbSet<ProductDetail> ProductDetail { get; set; }
       public DbSet<NoteBook> NoteBook { get; set; }
       public DbSet<Note> Note { get; set; }
       public DbSet<PublishedNotes> PublishedNote { get; set; }
    }
}