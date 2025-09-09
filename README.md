# Timeline:
  - This is my third attempt at a program that could enable me to emulate and eventually be able to stop using a paper planner.
    - The first two attempts were relatively short lived, and amounted to nothing. The knowledge I'd learned from them was not the most accurate, and I wound up having to rewrite the code from them for this attempt as a result.
    - Both of the first two attempts took place either half way through last year, or a year before that (respectively). I ceased working on them due to workload, a lack of true planning, and wanting to prioritize other projects.
    - This third attempt began in the last two weeks of summer break (though I'd been thinking about and drawing ideas a week or two before that), and has continued into the fall semester of this academic year, and have worked on it
        somewhat frequently since - but even then, it's not something I can prioritize all of or even most of my time to.
  - I am using this project as a way to learn how to develop software programs, and I am doing this in WPF and C#. C# is the programming language I am most comfortable with.
      - This project will also help me complete the research I need to do as part of my college's honors program, as I need to make a software program to conduct my research, and I plan to use WPF. 

# Goals:
  - When I began the first attempt into this project, I had big plans, and poorly defined ideas for what I wanted to create, but the core of it stayed consistent:
      - I wanted to create a program where I could create files in which I could list and break down my tasks into smaller "sub-tasks" or "subtask information" that could then too be further broken down as needed into sub-tasks of their own.
        - I wanted to be able to expand or collapse these tasks so their sub-tasks would not be visible if I didn't want them to be, thereby decreasing visual clutter and help me figure out my most immediate goals.
      - Examples of subtasks or "subtask-information" would include: 
        - I wanted to be to add more details to these tasks, such as being able to put images or descriptions that could further explain them.
        - I wanted to be able to link to other "plans" or tasks in "plans" (the file I want to create) if I felt a task could be further broken down into smaller parts (such as when I'm planning out features for games).
        - I wanted to be able to move these tasks around within their respetive plans, and to move the user's perspective around such that there could be tasks in the file that are not displayed in the plan, enabling rearranging of items on screen, and allowing for more tasks to be packed into one plan.
  - Here is an example of what I had envisioned from my second attempt during Sophomore year. I used example tasks based on some of my projects and workload at the time:
![EarliestPlanningProgramDrawing](ImagesForGithubOfPlanningProgram/EarliestPlanningProgramVersion.png)
  - In this most recent iteration, I realized that I wanted this tool to replace my paper planner, as having to tie tasks to specific days if they didn't need to be (ex: if they were a project I wanted to work on in general), and seeing a day in my planner filled with writing - even if that writing is 75% description - tends to stress me out and make me feel overwhelmed.
      - After this realization, I made another visualization of what I was hoping to create, as shown below (apologies for the poor drawing and lack of alt-text right now, I plan to add them later):
  ![UpdatedPlanningProgramDrawing](ImagesForGithubOfPlanningProgram/UpdatedPlanningProgramDrawing.jpg)


# How I'm going to accomplish this/How Will It All Work:
  - There are largely two parts that I don't fully think are "self-explanatory" as to how I intend to accomplish what I'm doing. These are how I will be saving the plans and tasks, and how I will be displaying them to the screen in the calendars.
        - When I say self-explanatory, I don't mean literally self explanatory, but I mean more just that in the case of a good portion of my code, such as with the models and view models and views,
            can be explained as being such - and what can't be explained fully within those files as being part of MVVM (or may come across as questionable choices), are choices I will touch on in a later section.
      - In the case of the former, my current plan is to save the data to XML, in part because I feel more comfortable working with XML than JSON or some other data format from when I was working on mods for Baldur's Gate 3.
          - Each "Plan" (the file) will have a list of "tasks" (the highest level of items stored in the plan, and the only items that the plan view model will actually "know of"), and each of those tasks will have a list of
              tasks that they "know" of too. This is - obviously- a lot of lists or collections (even if not lists) to store, so I will put a pin in this topic for later.
          - Each item is intended to have a GUID for differentiation - another thing I picked up from how Baldur's Gate 3 stored objects in its files.
      - In the case of the latter, my current plan is that when what are currently referred to as DateDuration\[Model/ViewModel/View\] in my code are saved, they will fire an event that tries to add or update the information for the 
          data they each hold (currently, a name of the task that is their parent, a guid for themselves, the date the task "starts," the date the task "ends," the filepath for the file they are in
          and whether or not the task is completed) in an SQLite database.
          - This database is what contains the information that the calendar (a monthly and a weekly version) will display. If users click on a task in the calendar, a box will show up and prompt them to open the task (if
              there is a corresponding file and task that go with the data stored in the database), to create a task that uses the data in the database in a file of the user's choice (if the task/file are not found), or to delete the
              task from the database (if the task/file are not found).

  - While plans are their own files, there should only be one database for the entire program.

# What Resources I have used:
  - As this project has had a massive learning curve, and is pretty large in terms of scope, I have had to consult a variety of resources to get it to even this point.
      - For help and advice, I have gone to one of the computer science professors at my college, as he teaches a course on software development.
      - I have also frequently used articles on CodeProject in order to better understand WPF and some of the code I am writing.
      - I have also made frequent usage of the documentation for C# and WPF, as one of my first resources when things go wrong.
  - I have used a lot of online resources - including things like stack overflow questions - to figure out how to do what I want to do, and to understand what's going wrong
      - Much of the code I have used from online has links to where I got them from commented in my code, so I can go back and see what others did, and to better understand why I did what I have done.
          - My general rule is to try to understand any code I have copied, but sometimes it is best to know where I got code from too, as code from online can sometimes be unoptimized or have its own issues
          - I frequently found myself having to tweak code from online to fit my own needs, so nothing is fully taken from online and copied as it is completely.
          - I try to learn from the code I take from online, rather than to just copy paste it - if I'm really not understanding something, I'll even go so far as to write it line by line until I understand it.

# Things that are completed, or done for now:
  - Models and view models and views for the tasks and sub-tasks - done for now
  - Getting tasks for calendars - the methods for fetching the tasks and querying the database for this purpose is complete
  - Plan view - done for now but will return to later
  - Can add sub-items to tasks, and can add items to plans

## Images Of What Works
![This is one example image of the Monthly Calendar at work](ImagesForGithubOfPlanningProgram/MonthlyCalendarWithTasks1.png)
![This is a second image of the Monthly Calendar at work- where I went back one month, to show the other test tasks](ImagesForGithubOfPlanningProgram/MonthlyCalendarWithTasks2.png)
![This is an image of the Weekly Calendar with tasks displaying](ImagesForGithubOfPlanningProgram/WeeklyCalendarWithTasks.png)
![This is an example of what I would like to be able to do with individual plans, using my current workload for today at the time of writing this readme](ImagesForGithubOfPlanningProgram/ExamplePlanToday.png)

# Needs work:
  - Calendars - need to make more universal (rather than two different files, with lots of shared functionality and minute differences)
    - Calendars also have a lot of code in the code behind, and I'm not quite sure if that breaks MVVM principles.
      - I believe the way that the "CurrentDate" property/dependency property is handled currently breaks MVVM, but that was a choice that must also be pinned for later.
      - Calendar controls are largely adapted from someone else's code, and so I need to do some more work on the code to make it all work.
      - I would also like to switch out the calendar control combo boxes for a date picker, which will just be much easier, and will ideally enable me to avoid some issues
          with certain events getting called when the date is set programattically rather than by the user.
  - Plans - right now the model stores a list of the task view model, thereby breaking MVVM - I need to figure out how to fix this, while also ensuring that the plan control is not having to
      display the model, rather than the view model.
  - The Drag and Drop will at some point need to be changed to ensure that
  - The viewmodel for the mainwindow will need to be changed to ensure that I can have multiple plans open at once (even if only one is displayed at a time) like in a tab control. I'd _like_
      to only have the controls for one plan created at a time for optimization purposes, but I don't know if the tab control will do that by default.
  - Drag and Drop is done *for now*, but it will need some work as I'm still getting errors every every once in a while while trying to use the program to make example images


# Next Goals In Order:
  - Make it so can save and load data into/from xml for the plans. This is my next priority for this project, and hopefully/ideally, by the time that someone sees this code,
      this is done, but I can't guarantee this, due to school and other personal projects (also with the goal of learning how to do things- one example including my ideas for
      redoing a dialogue system I made in freshman year to be much better) potentially cropping up or taking priority if I get sufficient things done on said personal projects.
  - Displaying multiple plans at a time, saving and loading them
  - Deleting elements and sub-items
  - Being able to move around in the canvas, such that there is technically "infinite space" to be used in the plans, rather than just what can be displayed on a canvas control

# Optimization/Weird Code:
  - The fact that a lot of the code in the calendars is in the code behind is a choice made to save time (by not refactoring) - I intend to refactor the code there eventually,  
      but since they work right now, they are not my immediate priority.
  - The fact that when CurrentDate is set, the tasks for the month before and after that date are fetched was done intentionally to ensure that when buttons were pressed,
      the tasks displayed would update. I intend to fix this when I refactor the calendars, but I wanted to prioritize a minimum functioning product right now so I could get this done.
  - With the amounts of lists being displayed, and nested lists (because a task parented by a task, parented by a plan - each of which have a list; that's 3 lists right there), I've been thinking about how I could optimize the tasks if I needed to, so I could use
      fewer list view controls. My current idea, is rather than nesting the list views, to have 3 collections per task: the subitems collection, a collection of ints that display if those are open (1) collapsed (0) or lack items (-1), and a list of integers that contains the
          indentation from the parent, to be multiplied with some constant value to offset the sub-items from their parents (thereby giving the illusion of them being nested). This would remove all of the list views from any-subtasks.
        - However, this would require a lot of work on my part, so, because I'm not seeing too big of optimization issues while debugging right now, it's not my priority, and I am instead prioritizing saving and loading data. 




