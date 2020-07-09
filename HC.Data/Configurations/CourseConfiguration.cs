using System;
using System.Collections.Generic;
using System.Text;
using HC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Data.Configurations
{
    class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasOne(x => x.Creator)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CoursesToStudents)
                .WithOne(x => x.Course);

            builder.Property(x => x.ImgUrl)
                .IsRequired();

            builder.HasData(
                new Course
                {
                    Id = 1,
                    Name = "Machine Learning with TensorFlow on Google Cloud Platform Specialization",
                    Info = "What is machine learning, and what kinds of problems can it solve? What are the five phases" +
                           " of converting a candidate use case to be driven by machine learning, and why is it important" +
                           " that the phases not be skipped? Why are neural networks so popular now? How can you set up" +
                           " a supervised learning problem and find a good, generalizable solution using gradient descent " +
                           "and a thoughtful way of creating datasets? Learn how to write distributed machine learning models" +
                           " that scale in Tensorflow, scale out the training of those models. and offer high-performance predictions." +
                           " Convert raw data to features in a way that allows ML to learn important characteristics from the data and" +
                           " bring human insight to bear on the problem. Finally, learn how to incorporate the right mix of parameters" +
                           " that yields accurate, generalized models and knowledge of the theory to solve specific types of ML problems." +
                           " You will experiment with end-to-end ML, starting from building an ML-focused strategy and progressing into model" +
                           " training, optimization, and productionalization with hands-on labs using Google Cloud Platform.",
                    CreatorId = 1,
                    ImgUrl = "https://www.essentialguru.org/wp-content/uploads/2020/04/Machine-Learning-with-TensorFlow-on-Google-Cloud-Platform-Review.png"
                },
                new Course
                {
                    Id = 2,
                    Name = "Excel to MySQL: Analytic Techniques for Business Specialization",
                    Info = "Formulate data questions, explore and visualize large datasets, and inform" +
                           " strategic decisions.In this Specialization,you’ll learn to frame business" +
                           " challenges as data questions.You’ll use powerful tools and methods such as " +
                           "Excel,Tableau,and MySQL to analyze data,create forecasts and models,design " +
                           "visualizations, and communicate your insights.In the final Capstone Project,you’ll" +
                           " apply your skills to explore and justify improvements to a real - world business" +
                           " process.\n The Capstone Project focuses on optimizing revenues from residential property,and Airbnb, " +
                           "our Capstone’s official Sponsor,provided input on the project design.Airbnb is the world’s largest" +
                           " marketplace connecting property - owner hosts with travelers to facilitate short - term rental transactions." +
                           "The top 10 Capstone completers each year will have the opportunity to present their work directly to senior data " +
                           "scientists at Airbnb live for feedback and discussion.",
                    CreatorId = 1,
                    ImgUrl = "https://smartprogress.do/uploadImages/001063504_l_crop.jpg"
                },
                new Course
                {
                    Id = 3,
                    Name = "C# Programming for Unity Game Development Specialization",
                    Info = "This specialization is intended for beginning programmers who want to learn how to program Unity games using C#. " +
                           "The first course assumes no programming experience, and throughout the 5 courses in the specialization you'll learn " +
                           "how to program in C# and how to use that C# knowledge to program Unity games. The C# and Unity material in the first " +
                           "4 courses in the specialization is slightly more comprehensive than the content in the first 2 game programming courses" +
                           " at UCCS.“Unity” is a trademark or registered trademark of Unity Technologies or its affiliates in the U.S.and elsewhere." +
                           "The courses in this specialization are independent works and are not sponsored by, authorized by, or affiliated with Unity" +
                           " Technologies or its affiliates.",
                    CreatorId = 1,
                    ImgUrl = "https://img-a.udemycdn.com/course/750x422/1399296_e175_3.jpg"
                }
            );
        }
    }
}
