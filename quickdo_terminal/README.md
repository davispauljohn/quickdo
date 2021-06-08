# q[uick]do

A simple task list and activity log for your terminal. 

## Commands
Command                                       |Syntax                             |Options    
----                                          |----                               |----
Display and filter tasks                      |?                                  |`default` - Display task list
Display task at rank 1                        |!                                  |`default` - `qdo ? -t0 -q1` 
Move task to rank 1                           |![__rank__:_int_]                  |
Display task created most recently            |+                                  |`default` - `qdo ? -t0 -q1` 
Create task with status TODO                  |+[__description__:_string_]        |
Display task most recently set to DONE        |x                                  |`default` - `qdo ? -t0 -q1` 
Set the specified task status to DONE         |x[__rank__:_int_]                  |
Display task most recently set to NOPE        |-                                  |`default` - `qdo ? -t0 -q1` 
Set the specified task to NOPE                |-[__rank__:_int_]                  |     
