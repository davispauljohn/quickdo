# q[uick]do

A simple task list for your terminal. 
Data stored locally in json file created daily and labelled atomically


Download and add `qdo` to your path to begin.

## Commands
Command                                       |Syntax                             |Options    
----                                          |----                               |----
List tasks                                    |?                                  |`default` - Display task list with default options.<br />By default this is: `do ? -t0 -s DO -q0`    
Show task on top of the queue                 |!                                  |`default` - `do ? -t0 -q1` 
Move the specified task to top of the queue   |![__rank__:_int_]                  |
Show task created most recently               |+                                  |`default` - `do ? -t0 -q1` 
Create a new task with status DO              |+[__description__:_string_]        |
Show task most recently set to DONE           |x                                  |`default` - `do ? -t0 -q1` 
Set the specified task status to DONE         |x[__rank__:_int_]                  |
Show task most recently set to NOPE           |-                                  |`default` - `do ? -t0 -q1` 
Set the specified task to NOPE                |-[__rank__:_int_]                  |     
