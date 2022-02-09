# User Guide

This is primarily a guide for content authors/approvers, but will also be useful for developers and testers. It explains how to edit content in Contentful to update the web site.

## Contentful

[Contentful](contentful.com) is the content management system (CMS) for the [Find Employer Schemes website](https://find-employer-schemes.education.gov.uk/) (FES). Log in to Contentful with your given credentials.

## Roles

The content team are given the 'author' and 'editor' roles. This means every team member can both edit and publish content. It is up to the team, to ensure that all content edits are reviewed by someone other than the author, before publishing.



## Environments

Developers and testers will be given the 'administrator' role. This allows the use of different Contentful environments.

When developing and testing new features, schema and content changes can be made, without having to touch the 'master' production environment.

Due to a limitation in the number of Contentful environments available to us, there isn't a 1:1 mapping of environments. The environments are mapped thus:

| FES Environment   | Contentful Environment |
|-------|----------------|
| prod  | master         | 
| pp    | pp             | 
| test2 | test           | 
| test  | test           | 
| at    | at             | 

## TODO

Preview capability
CTA box.
Links with test that ends with "(opens in new tab)", will open in a new tab
How to add YouTube video
Roles (if we split editor/author)
Details page override
Additional footer