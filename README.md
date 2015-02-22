# ProjectManagementSystem
Undergraduate class project. An MVC Application using ASP.NET and Entity Framework. All design decisions were left
up to the group members to decide and research. I was solely responsible for the backend (the controller and the model)
as well as for high-level software architecture and engineering.

To Use the App:
	1) The username and password are identical.
	2) Three usernames illustrate the application authorization (different permissions were
crudely enforced). They are... admin, ambassador, contributor.

Project Goal: 
	Create an application that allows CommerceBank to manage projects that involve university students.

Project Duration:
	September 10, 2014 - December 8, 2014.

My Background at the Time: 
	At the time of application development, my experience was limited. I had completed many
toy projects in my courses. However, I hadn't produced a software system that involved software
engineering, API usage, architectural considerations, etc. This was my first major project. You'll
notice that my code doesn't follow any design patterns. I hadn't gone through a course that introduced
me to design patterns at this point, so many strong dependencies are present. The code reveals my 
lack of experience at the time. However, this was a good project and I took on many all-nighters 
to get it finished.

Summary of Design Process:
	- Needed to ensure MVC compliance by strict separation of Model, View and Controller.
	- MVC compliance was achieved by use of a layered architecture with the Entity Framework
at its base, a Business Logic layer coupled with a custom mapper implementing the AutoMapper API,
the Controller and then the View.
	- Potentially harmful Domain Models (DM) were transformed in the Business
Logic layer to harmless View Models (VM) to be passed to the Controller ensuring that
data manipulation occurred exclusively in the Model.
	- A crude authorization scheme was quickly designed to establish authorization spaces independent
of one another. If privilaged users ended up in reduced privilage space, software redirected them to
correct privilage level. If reduced privilage user ended up in privilaged user space, system forced
them to log out to ensure data integrity.  

Design Decisions and Responsibilities:

	During this project, I was responsible for the entire back-end, as well as for the software
engineering and architectural considerations. With my limited knowledge at the time and about three months
to accomplish the task, that was sizeable.	

	In order to comply with the MVC architecture, I had to ensure strict separation of the model, view
and controller. This was important, because compliance with MVC meant ensuring that domain
models (models that have the capability of altering the models state and, therefore, our users data) aren't 
present within the controller. This design decision meant that a mapping interface was required to transform
dangerous domain model(DM) objects into harmless view model(VM) objects. I called this layer the Business Logic layer
of the Database Interface. This layer performs mappings of DM objects to VM objects using the AutoMapper API.
As a result, the controller only has exposure to VM objects, which are harmless. Therefore, the models received
by the views are also harmless and can't be used to alter the data of the Model itself, ensuring that the 
business logic desired by our customers is enforced for future developers.

	In an, albeit, code-crazy attempt to enforce strict separation of accounts I produced Views for 
each level of authorization allowing for customization of each respective view for contributors, ambassadors
and administrators (the three client-specified roles). Upon inspection of the result, I'm sure there's a better
way to enforce authorization. However, the time constraints when making those decisions were so great that I 
decided to iteratively go through each controller associated with each authorization space and do checks on 
the stored user access privilage level. In order to ensure that execution time would receive minimal impact,
I checked for the most common case first, followed by erroneous cases. If the authorization level of the 
current user was greater than that of the current authorization space (i.e, an admin ended up in the contributor
authorization space) I redirected that user to the appropriate location. This was possible, because the customer-
defined features included an admin that could do anything that an ambassador and contributor could do and the
ambassador could do anything the contributor could do. So, the administrator would always have equivalent views
to those authorization spaces that were less capable. If, however, a low-access privilage user entered admin space,
my crude system would detect that error and would force the lower-authorization user to log out.

	Other than that, Controller design was fairly straight forward. Controllers simply needed to receive 
input from the Database Interface (VM objects), check the validity of those objects and then route them to
the correct view.

	For database access, I used Entity Framework, because of its simplicity as well as 
the massive reduction in coding time allowed by its use. Coding our own interface would've added a 
significant amount of work reducing the number of customer-specified features that 
we'd be capable of implementing. As a result, Model access was fairly trivial. The most difficult
aspect of Model design was enforcing correctly performing mappings of DM objects to VM objects.
What was most difficult about this was the case of multi-DM to VM mappings. My solution was to use
lists of DM objects as input into a custom mapper function. This solution was sufficient.