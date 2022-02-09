# User Guide

This is primarily a guide for content authors/approvers, but will also be useful for developers and testers. It explains how to edit content in Contentful to update the web site.

## Contentful

[Contentful](https://www.contentful.com) is the content management system (CMS) for the [Find Employer Schemes website](https://find-employer-schemes.education.gov.uk/) (FES). Log in to Contentful with your given credentials.

## Roles

The content team are given the 'author' and 'editor' roles. This means every team member can both edit and publish content. It is up to the team, to ensure that all content edits are reviewed by someone other than the author, before publishing.

## Editing

For a general guide to Contentful, consult the [Contentful help center](https://www.contentful.com/help/).

### Call to action box

### Links

Links with text that ends with "(opens in new tab)", will open in a new tab

### Embedded YouTube videos

## Content Types

### Scheme

The scheme content type contains the content and metadata associated with a training scheme.

Content items of this type are used to
* render the scheme details on the homepage
* drive the filtering on the homepage
* render each individual scheme details page

Schemes can be created, edited and deleted, and the web site will update accordingly.

The editor has help text for each field, but some fields require more explanation, which is given here:

#### Details page override

Any content in the 'Details Page Override' field, overrides the usual composition of the scheme details page from the normal set of fields, such as 'Description' and 'Cost'.

Instead the main content of the scheme details page is taken from this field. It is used when the required content for the scheme doesn't match the usual format, such as the content for 'Training outside of employment'.

#### Additional footer

#### Size

The size field determines the ordering of the schemes on the home page and in the other schemes section on scheme details pages. The schemes with the bigger sizes are rendered higher up.

The existing schemes sizes have been set to the number of citizens currently on the schemes, but the size field can be aritrary.

#### Filter metadata

#### Case studies

### Page

Pages can be created, edited and deleted, and the web site will update accordingly.

Pages are located at https://find-employer-schemes.education.gov.uk/page/{page-url}, where `{page-url}` is taken from the Url field of the page.

#### Error injection page

There is a special [error injection page](https://find-employer-schemes.education.gov.uk/page/error-check), that allows us to check the error page.

### Case study page

## Publish and Preview

Production content is edited in a single environment. (See below for details on environments, but content editors will see only see the single 'master' production environment.)

Draft pages and schemes can be previewed in the production environment by clicking the 'Open preview' button. That will open a browser tab showing the draft scheme or page (including draft versions of any referenced content types, such as draft filters).

Once draft content is finalised and has had a second person review and approve it, the item can be published.

The web site updates on a schedule. The current schedule for production is for updates to occur every half hour from 6:00 to 23:30. (The schedule is configurable and can be changed on request.)

That means e.g. if you publish content at 9:15, it will appear on the web site at 9:30.

It also means that if you have a set of content updates that you want to be published together, you can publish them all within the window and they will all be published simultaneously.

## Sitemap

The [sitemap](https://find-employer-schemes.education.gov.uk/sitemap.xml) is updated automatically (on schedule) when pages or schemes are published or unpublished.

## Environments

Developers and testers will be given the 'administrator' role. This allows the use of different Contentful environments.

When developing and testing new features, schema and content changes can be made, without having to touch the 'master' production environment. E.g. when schema or content changes are made in the Contentful test environment, the changes are visbible in the FES test (and test2) environment.

### Environment mapping

Due to a limitation in the number of Contentful environments available to us, there isn't a 1:1 mapping of environments. The environments are mapped thus:

| FES Environment   | Contentful Environment |
|-------|----------------|
| prod  | master         | 
| pp    | pp             | 
| test2 | test           | 
| test  | test           | 
| at    | at             | 

### Environment update schedules

The pp environment updates on the same schedule as production. The other environments update every five minutes from 7:00 to 18:55 to allow for quicker development and testing.

> **_NOTE_**
> 
> Contentful currently has rate throttling set at 13 hits/sec.
> 
> With 2 instances per environment, at 0,30, there will be 10 hits/sec.
> 
> If the throttling rate is reduced, or the number of environments/instances increases, we'd have to stagger updates across environments (which we couldn't do initially, as the release pipeline requires config to be the same across environments).