# MTCG

This project has been part of the software engineering lecture.

The core of this project is a concurrent REST server which has been created without any frameworks like Asp.net.

I designed the server in a way, so it is easy to add new routes with the help of attributes. The routes also support query and path parameters.

The workings of the DAL have been inspired by the Entity framework and uses the unit of work pattern with a simple or mapper.

Concurrency of the databse is handled with the principle of optimistic concurrency.
