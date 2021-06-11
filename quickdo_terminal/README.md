# q[uick]do

A simple task list and activity log for your terminal. 

## Commands
Command                                       |Syntax                             |Options    
----                                          |----                               |----
Display and filter tasks                      |?                                  |`default` - Display task list
Display task created most recently            |+                                  |`? -d0 -s1 -t1 -d` 
Create task with status TODO                  |+[__description__:_string_]        |
Display task at rank 1                        |!                                  |`? -d0 -r1`  
Move task to rank 1                           |![__rank__:_int_]                  |
Display task most recently set to DONE        |x                                  |`? -d0 -s2 -t1 -d`  
Set the specified task status to DONE         |x[__rank__:_int_]                  |
Display task most recently set to NOPE        |-                                  |`? -d0 -s3 -t1 -d` 
Set the specified task to NOPE                |-[__rank__:_int_]                  |     
