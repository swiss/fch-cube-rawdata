# Introduction

This library can be used to create RDF triples in the context of cubes (see: https://cube.link).
The library should be used together with fch-cube-dimension.

# Development Workflow

To publish a new version of the NuGet package, proceed as follows.

* apply and push your changes
* define and describe the new version in CHANGELOG.md
* push the corresponding label with 'git tag vx.x.x' and 'git push origin v.x.x.x'
* go to GitHub -> Actions -> 'Build and Publish to NuGet.org' and trigger a run while specifying the correct GIT label
