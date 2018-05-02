# basic-courses-api
A Web API that exposes endpoints for creating Courses, Chapters and Lessons. 

1 Course -> n Chapters 

1 Chapter -> n Lessons


1 User -> n LessonCompletions

1 User -> n Achievements

Automatically validates achievements (# of lessons completed, # of chapters completed,...)

Runs on .NET Core, EF Core and SqlLite.
