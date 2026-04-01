# Prompt: Selecting Skills for a Task (Response in JSON)

Act as an expert task planner.
I will provide you with two elements:
1. **The user's task**: a description of what the user needs to accomplish.
2. **The system skills**: a list of available capabilities that can be used to carry out different actions.

Your goal is to **analyze the task and select the most relevant and necessary skills** to complete it.

You must respond with the selected skills, one per line. For example:
comparison-summary
general-reasoning

## Input data

### User task
Search the web for comparisons between Unity IAP and RevenueCat and send me a clear summary with pros and cons.

### System skills

{Skills}