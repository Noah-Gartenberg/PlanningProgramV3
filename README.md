# Goals and Planning:
  - When I began my first attempt, I had big ideas that I had no clue how to create. The core of the program has stayed largely the same in each attempt:
    - I wanted to create a program where I could create files in which I could list and break down my tasks into smaller "sub-items" that could then too be further broken down - as needed - into sub-items of their own.
      - I wanted to be able to expand or collapse these tasks so their sub-tasks would not be visible if I didn't want them to be, thereby decreasing visual clutter and help me figure out my most immediate goals.
    - Examples of subtasks or "subtask-information" would include: 
      - I wanted to be to add more details to these tasks, such as being able to put images or descriptions that could further explain them.
      - If these items were in some way time sensetive, I wanted to be able to display them on a calendar.
      - I wanted to be able to link to other "plans" or tasks in "plans" (the file I want to create) if I felt a task could be further broken down into smaller parts (such as when I'm planning out features for games).
      - I wanted to be able to move these tasks around within their respetive plans, and to move the user's perspective around such that there could be tasks in the file that are not displayed in the plan, enabling rearranging of items on screen, and allowing for more tasks to be packed into one plan.
- Here is an example of what I had envisioned from my second attempt during Sophomore year. I used examples pulled from my workload and projects from the time:
![EarliestPlanningProgramDrawing](ImagesForGithubOfPlanningProgram/EarliestPlanningProgramVersion.png)
  - At the start of August, I realized that I wanted this tool to replace my paper planner to help me stay better organized without stressing myself out needlessly because the box for today got filled up with lots of description for the one thing I had to do (exagerating for the sake of the example).
    - After this realization, I made another visualization of what I was hoping to create, as shown below (apologies for the poor drawing and lack of alt-text right now, I plan to add them later):
  ![UpdatedPlanningProgramDrawing](ImagesForGithubOfPlanningProgram/UpdatedPlanningProgramDrawing.jpg)
- I also need to create a software program to use during my honors research which I will be conducting during the school year over the next two years, so I wanted to learn software development so I could complete this task. As I was most comfortable with C#, and had previously used WPF for this project, I felt it would be best for me to stick with what I know/know best, as my next semester will be particularly challenging


# What features will there be:
<details>
  <summary>What features are there/will there be?</summary>
  <ul>
    <li>The program is meant to serve as a replacement for a user's paper planner (for my own, if for nobody else)
      <li>My paper planner tends to fill up, resulting in it looking like this: [show image]
        <ul>
          <li>This stresses me out, because regardless of if half of the writing is for things I want to get to, an assignment I needed a second line to describe, or something that I want to make note of for another assignment, it looks like I have more work.</li>
          <li>Additionally, although the calendar in my planner is useful for writing down tasks with deadlines, I have found I struggle to consistently use it, due to the small spaces in which I would have to jot notes down in, and the amount of work I get meaning it fills up quickly</li>
        </ul>
      </li>
      <li>I specifically wanted to make a planner
        <ul>
          <li>I chose not to use Obsidian or another such planning software because I have found they tend not to lend themselves to the way in which I make notes of my work, where I write a short name of the task, and then I break it down into smaller tasks below that</li> 
          <li>[picture example]</li>
          <li>I chose not to use a calendar because putting something in for a timeslot or on a day makes me feel like I _have_ to get that thing done on that day and/or at that time, and if that is not so, then that just causes me needless stress</li>
        </ul>
      <li>Features I wanted to include:
        <ul>
          <li>**Plans**, that can store tasks within them
            <ul>
              <li>**Tasks** (the base item that will be in a plan) should be able to move around in the plans, to be able to be marked as completed, and to be able to collapse and reveal below them a list of descriptions, sub-tasks, or dates between which tasks should be completed.</li>
              <li>[Show picture of tasks, and how they'll work]</li>
            </ul>
          </li>
          <li>There should be two calendars - one that displays a month at a time, and one that displays a week at a time - that show tasks that must be completed between dates within the month or week that are being displayed. When users click on those tasks, they should be brought to the corresponding plans the tasks are in. 
        </ul>
      </li>
    </li>
  </ul>
</details>

# How will it work?
- I am using the MVVM programming pattern, along with C#, and the WPF framework, to create this program, and most of the functionality.
- To store data, I will be using XML to store the individual plans, and an SQLite database to store the data for a calendar to display timesensetive tasks
  - There will be more information on the SQLite database's design soon.
  - SQLite was used because I needed something that could store lots of data, and that I could query to find, update, add, or delete data from, while also allowing users to store their data on their computers

<details>
  <summary>In depth explanation</summary>
  <ul>
    <li>All of the controls and the design of the app has been made using the WPF framework.</li>
    <li>I programmed much of the functionality of the plans and the app as a whole using the MVVM programming pattern.</li>
    <li>Though I didn't know this at the time, each task that can be in the plan is implemented in a way that can be described as a non binary tree (as in: more than two branches per tree).</li>
    <li>Plans are saved to XML files, as I have worked with XML before while making mods for Baldurs Gate 3, and because I needed a way to save files that would allow me to physically read them, manually edit them, and to collapse sections if they are not relevant to what I am looking for</li>
    <li>I will be storing the tasks that are to be displayed to the calendars in an SQLite database.
      <ul>
        <li>SQLite is being used because I needed a way for the program to store potentially a lot of data in a way that can handle relationships between objects in that data, and in a way that can be queried to ensure fast lookup, add, edit, and removal times. I did not feel a full SQL database would be necessary for this program, and I would like to keep files on peoples' computers if I can.</li>
      </ul>
    </li>
    </ul>
      <li>Everything is a work in progress, so everything is subject to change.</li>
    </ul>
</details>

# What Resources I have used:
  - As this project has had a massive learning curve, and is pretty large in terms of scope, I have had to consult a variety of resources to get it to even this point.
      - For help and advice, I have gone to one of the computer science professors at my college, as he teaches a course on software development.
      - For help and advice on databases, I have gone to one of my professors who has taught me about working with databases. 
      - I have also frequently used articles on CodeProject in order to better understand WPF and some of the code I am writing.
      - I have also made frequent usage of the documentation for C# and WPF, as one of my first resources when things go wrong, or just to understand the features of WPF and C#.
  - I have used a lot of online resources - including things like stack overflow questions - to figure out how to do what I want to do, and to understand what's going wrong
      - Much of the code I have used from online has links to where I got them from commented in my code, so I can go back and see what others did, and to better understand why I did what I have done.
          - My general rule is to try to understand any code I have copied, but sometimes it is best to know where I got code from too, as code from online can sometimes be unoptimized or have its own issues. Or, if I wish to go back to it in cases I forget why I did something, I would like to be able to do so. For this reason, I have commented into my code links where I got some information from where applicable.
          - I frequently found myself having to tweak code from online to fit my own needs, so nothing is fully taken from online and pasted as it was completely.
          - I try to learn from the code I take from online, rather than to just copy paste it - if I'm really not understanding something, I'll even go so far as to write it line by line until I understand it.

## Images Of What Works
![This is one example image of the Monthly Calendar at work](ImagesForGithubOfPlanningProgram/MonthlyCalendarWithTasks1.png)
![This is a second image of the Monthly Calendar at work- where I went back one month, to show the other test tasks](ImagesForGithubOfPlanningProgram/MonthlyCalendarWithTasks2.png)
![This is an image of the Weekly Calendar with tasks displaying](ImagesForGithubOfPlanningProgram/WeeklyCalendarWithTasks.png)
![This is an example of what I would like to be able to do with individual plans, using my current workload for today at the time of writing this readme](ImagesForGithubOfPlanningProgram/ExamplePlanToday.png)


# Timeline (of attempts):
  - This is my third attempt at a program that could enable me to emulate and eventually be able to stop using a paper planner.
    - The first two attempts were relatively short lived, and amounted to nothing. The knowledge I'd learned from them was not the most accurate, and I wound up having to rewrite the code from them for this attempt as a result.
    - The first two attempts took place roughly half way through last year, and a year before that (respectively). I ceased working on them due to workload, a lack of understanding, and wanting to prioritize other projects.
    - This third attempt began in the last two weeks of summer break (though I'd been thinking up ideas, and researching ways to implement various parts a week or two before that), and has continued into the fall semester of this academic year. I've worked on it frequently since - but even then, it's not something I can prioritize all of or even most of my time to.
  - I am using this project as a way to learn how to develop software programs using WPF and C#.
      - This project will also help me complete the research I plan to do for my college's honors program. My research will require a software program, and I plan to make one for myself.
