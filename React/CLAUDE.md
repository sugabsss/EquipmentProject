 GLOBAL CONTEXT
This project consists of converting a legacy .NET WebForms system to a new architecture.
One rule that applies globally: when writing summaries for Enums, you must list all possible values in the documentation, as shown in the example below where the ShiftType enum and all its parameters are documented:
csharp/// <summary>
/// Worker shift type:
/// 1 - Day | 2 - Night | 3 - Mixed
/// </summary>
public EnumShiftType ShiftType { get; set; }

🎯 OBJECTIVE
Assist in converting WebForms screens to the new standard, strictly following the rules and patterns defined in the helper files.

⚠️ GLOBAL RULES

Never generate code before the context for the task has been defined
Never modify files without prior approval
Never generate code outside the defined standard
Never modify files that are not within scope
Always respect layer separation
Never assume context without confirmation
Never delete or edit files outside the defined scope
Reading and searching for files using commands like find or grep is allowed, but only within the current project
No changes may be made without first requesting confirmation


🧩 RULE PRIORITY
Rules must be followed in this order:

🔒 Restrictions (restrictions and allowed-files rules)
📂 Specific contexts (report or screen)
⚠️ Global rules

In case of conflict: always follow the higher-priority rule.

📂 AVAILABLE CONTEXTS
There are two main conversion types. Before any execution, the user must specify which one to use, and based on that you will load the corresponding files:
1. Reports
2. Conventional Screens

❗ DECISION RULE
Before starting any conversion task, you MUST ask:

What type of screen are we converting?
After the answer, load ONLY the corresponding files for that context.
Then ask: what is the name of the .aspx file for this screen?
Begin by analyzing all service calls made by the screen — both through helpers and buttons (e.g., Filter, Save, Delete when present) — as these will become the new endpoints to be created.
Present everything that needs to be changed, but do not make any changes without explicit approval.

If there are hints in the request, suggest an option — but still require confirmation.

⛔ EXECUTION BLOCK
It is forbidden to:

Generate code
Suggest implementation
Create structure

BEFORE:

Identifying the type
Confirming with the user
Analyzing the corresponding files


🔎 ANALYSIS MODE
Before implementing, you must:

Analyze the original WebForms screen
Identify data, actions, and behavior
Propose a conversion plan
Wait for approval


🔒 RESTRICTIONS
Restriction rules have maximum priority. If a request violates any restriction, refuse and explain why.

🧭 EXPECTED BEHAVIOR

Be conservative — don't invent
Ask when in doubt
Follow existing patterns
Prioritize consistency over creativity


READ PERMISSIONS (BASH)
Free execution of read-only commands is allowed without prior confirmation. This includes: directory navigation (cd, ls), file search (find), content search (grep), and file reading (cat, less, etc.).
Restrictions:

Navigation must stay within the current project folder
Accessing external directories is not allowed
No write or modification command may be executed without confirmation

Still prohibited without confirmation:

Editing files
Creating files
Deleting files
Running scripts