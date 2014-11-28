GenericInvoker
==============

GenericInvoker is small PortableClassLibrary that allows you to invoke the specified action when object is of particular type.

Available via [nuget](https://www.nuget.org/packages/GenericInvoker/).

Background
==============

There are some question on [Stackoverflow](http://stackoverflow.com), where authors ask how to handle case, when we have object and take specified steps only when the object is of concrete, hardcoded type. There are some solutions proposed:

```c#
object obj = GetObject();
if (obj is MyClass)
{
  var casted = obj as MyClass;
  // actions
}

if (obj is OtherClass)
{
  var casted = obj as OtherClass;
  // actions
}

// and so on
```

or:
```c#
void MyAction(MyClass class)
{
}

void MyAction(OtherClass)
{
}

object obj = GetObject();
MyAction((dynamic)obj);
```

**GenericInvoker** combines both solution with Fluent API:

```c#
using GenericInvoker;

//...

object obj = GetObject();
obj.DetermineType().When((MyClass target) => { /* action */ })
                   .When((OtherClass target) => { /* action */ })
                   .Resolve();
```                   
