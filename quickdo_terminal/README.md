# q[uick]do

A simple task list and activity log for your terminal. 

**Documents** are JSON files for storing Tasks and Logs. One Document is created automatically for each day of use.
```
//Document
{
    "tasks": [],
    "log": [],
    "datestamp": "2021-06-09"
}
```

**Tasks** are text descriptions in an ordered list with a gloablly unique **Id**, **Rank** and **Status**  
```
//Task
{
    "id": "c45ffac9-eb0b-4b95-8f41-9ce98c19b1f7",
    "status": "TODO",
    "rank": 1,
    "description": "Lisa needs braces"
}
```

One or more **Log** entries are added per command, containing a **Type** and **Timestamp**. Log entries can also be added manually
```
//Log
{
    "timestamp": "2021-06-09T22:50:34.7529712\u002B10 UTC\u002B10",
    "type": "TASKCREATED",
    "taskId": "c45ffac9-eb0b-4b95-8f41-9ce98c19b1f7",
    "value": ""
}
```

## Commands
Command                                       |Syntax                             
----                                          |----                               
Display and filter tasks                      |?                                  
Display task created most recently            |+                                  
Create task with status TODO                  |+ [__description__:_string_]        
Display task most recently set to DONE        |-                                  
Set the specified task status to DONE         |- [__rank__:_int_]                  
Display task most recently set to NOPE        |x                                   
Set the specified task to NOPE                |x [__rank__:_int_]                  
Display task at rank 1                        |!                                  
Move task to rank 1                           |! [__rank__:_int_]                    

## Statuses
**TODO** - The default **Status** of a **Task** representing pending work

**DONE** - Positive conclusion

**PUSH** - Deferred and migrated to the next **Document** when it is created

**NOPE** - Negative conclusion

## Log Types
**DOCUMENTCREATED** - Added during the creation of the document. The first entry in all documents.

**QUERYEXECUTED** - Added when tasks are fetched

**TASKCREATED** - Added when tasks are created

**TASKCANCELLED** - Added when tasks are cancelled

**TASKCOMPLETED** - Added when tasks are completed

**TASKFOCUSED** - Added when tasks are focused

**TASKPUSHED** - Added when a task's rank is pushed

**RANKCHANGED** - Added when a task's rank is updated

