# Swastika-Heart

[![CodeFactor](https://www.codefactor.io/repository/github/swastika-io/swastika-io-heart/badge)](https://www.codefactor.io/repository/github/swastika-io/swastika-io-heart)
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FSwastika-IO%2FSwastika-IO-Heart.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FSwastika-IO%2FSwastika-IO-Heart?ref=badge_shield)
[![Build Status](https://travis-ci.org/Swastika-IO/Swastika-IO-Heart.svg?branch=master)](https://travis-ci.org/Swastika-IO/Swastika-IO-Heart)

## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FSwastika-IO%2FSwastika-IO-Heart.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FSwastika-IO%2FSwastika-IO-Heart?ref=badge_large)

## Reference
https://github.com/Swastika-IO/Swastika-IO-Heart-Sample

## Sample Code
*Create Models*

```c#
using System;

namespace SimpleBlog.Data.Blog
{
    public class Post
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string SeoName { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDateUTC { get; set; }
    }
}

namespace SimpleBlog.Data.Blog
{
    public class Comment
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDateUTC { get; set; }
    }
}

public class BlogContext : DbContext
{
    public DbSet<Post> Post { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public BlogContext()
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=demo-heart.db;Trusted_Connection=True;MultipleActiveResultSets=true");

        }
    }
}

```

*Using Heart*

### Create ViewModel Class
```c#
namespace SimpleBlog.ViewModels
{
    // Create ViewModel using Heart 
    public class PostViewModel: ViewModelBase<BlogContext, Post, PostViewModel>    
```

### Declare properties mapping from model to view
```c# 
        //Declare properties that this viewmodel need         
        public string Id { get; set; }
        [Required(ErrorMessage = "Title is required")]        
        public string Title { get; set; }        
        public DateTime CreatedDateUTC { get; set; }
        
        //Declare properties need for view or convert from model to view        
        public DateTime CreatedDateLocal { get { return CreatedDateUTC.ToLocalTime(); } }        
        public PaginationModel<CommentViewModel> Comments { get; set; }        

        public PostViewModel()
        {
        }

        public PostViewModel(Post model, BlogContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }
```

### Override methods
```c#
        //This method execute before this view saved to db
        public override Post ParseModel(BlogContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateUTC = DateTime.UtcNow;
            }
            GenerateSEO(_context, _transaction);
            return base.ParseModel(_context, _transaction);
        }
        
        // This method execute before load data to client view
        public override PostViewModel ParseView(bool isExpand = true, BlogContext _context = null, IDbContextTransaction _transaction = null)
        {
            // For example: We need list comments of this post for this view
            var getComments = CommentViewModel.Repository.GetModelListBy(
                    c => c.PostId == Id, // Conditions
                    "CreatedDate", OrderByDirection.Descending, // Order By
                    pageSize: 5, pageIndex: 0, // Pagination
                    _context: _context, _transaction: _transaction // Transaction
                    );
            if (getComments.IsSucceed)
            {
                Comments = getComments.Data;
            }
            return view;
        }   
```

## Using
*Save*
```c#
var saveResult = await post.SaveModelAsync();
```
*Get Single*
```c#
var getPosts = await PostViewModel.Repository.GetSingleModelAsync(p=>p.Id==1);
return View(getPosts.Data);
```

*Get All*
```c#
var getPosts = await PostViewModel.Repository.GetModelListAsync();
return View(getPosts.Data);
```

*Get All with predicate*
```c#
var getPosts = await PostViewModel.Repository.GetModelListByAsync(p=>p.Title.Contains("some text"));
return View(getPosts.Data);
```

*Get Paging*
```c#
var getPosts = await PostViewModel.Repository.GetModelListAsync("CreatedDate", OrderByDirection.Descending, pageSize, pageIndex);
return View(getPosts.Data);
```
*Get Paging with predicate*
```c#
var getPosts = await PostViewModel.Repository.GetModelListByAsync(p=>p.Title.Contains("some text"), "CreatedDate", OrderByDirection.Descending, pageSize, pageIndex);
return View(getPosts.Data);
```
