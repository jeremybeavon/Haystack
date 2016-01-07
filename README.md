# Haystack
Diagnosing and fixing regressions can take a lot of a developers time. This is a problem because:
* It's not fun
* It can take a lot of time
* It doesn't increase the value of the product/service
* It isn't always billable

Haystack is intended to help reduce the pain of diagnosing regressions. Sometimes diagnosing problems is like:
* Finding a needle in a haystack
* Finding a needle in a field full of haystacks
* Finding a single piece of hay in a stack of needles

Haystack provides a runner that logs information from various sources:
* **Amendments**: This utilizes an IL rewriter (Afterthought) that allows modification of properties, method or constructors
on a compiled assembly.
* **Code Coverage**: This runs code coverage. Currently only code coverage for .NET assemblies is supported (OpenCover).
In the future, code coverage for databases and browsers will be investigated.
* **Interception**: This adds logging to interfaces provided by a DI framework.
Currently Autofac, Castle Windsor, Ninject, StructureMap and Unity are supported.
* **Static Analysis**: This allows analysis of resources at various point during an execution.
Currently files, file content, and environment variables are supported.
* **Test Integration**: This allows custom code to be injected at various point in a test run.
Currently only NUnit is supported. In the future, MSTest and xUnit will be investigated.

Haystack also provides an analyzer, which takes log information from a successful run and a failing run and finds differences
between the two and where the failing run diverted from the original. It can also analyze source control to determine the
changes that caused the failure, if there were any.

# Usage

Still figuring that out
