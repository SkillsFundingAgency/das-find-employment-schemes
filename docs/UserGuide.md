# User Guide

This is primarily a guide for content authors/approvers, but will also be useful for developers and testers. It explains how to edit content in Contentful to update the web site.

## Contentful

[Contentful](https://www.contentful.com) is the content management system (CMS) for the [Find Employer Schemes website](https://find-employer-schemes.education.gov.uk/) (FES). Log in to Contentful with your given credentials.

## Roles

The content team are given the 'author' and 'editor' roles. This means every team member can both edit and publish content. It is up to the team, to ensure that all content edits are reviewed by someone other than the author, before publishing.

## Editing

For a general guide to Contentful, consult the [Contentful help center](https://www.contentful.com/help/).

### Call to action box

As there is no formatting support in Contentful for a call to action box, we instead mark the content we want to appear in a call to action box in a blockquote, with a given first line of `<cta>`.

### Links

Links can be added in the normal way for Contentful.

If you need a link to open in a new tab, append `(opens in new tab)` to the end of the link text (as recommended by GDS), and the web site will render the link so that it opens a new tab, if clicked.

### Embedded YouTube videos

If you want to embedd a YouTube video, copy the embed code for the YouTube video, making sure to select "Enable privacy-enhanced mode", and copy that into an edit box. Here's a [guide to get the embed code](https://axbom.com/embed-youtube-videos-without-cookies/).

Enabling the privacy-enhanced mode makes the embed code reference `youtube-nocookie.com`, which stops YouTube setting cookies on the site. (Our cookie page assumes no YouTube cookies are set.).

E.g.

```
<iframe width="560" height="315" src="https://www.youtube-nocookie.com/embed/xWUFLAAc4TY" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
```

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

Additional footer content to render in the footer, _above_ the usual footer content. See the Apprenticeship scheme for an example.

#### Size

The size field determines the ordering of the schemes on the home page and in the other schemes section on scheme details pages. The schemes with the bigger sizes are rendered higher up.

The existing schemes sizes have been set to the number of citizens currently on the schemes, but the size field can be aritrary.

#### Filter aspects

The Motivations, Pay and Scheme Length filter aspects referenced by a scheme determine whether the scheme is part of the result set, when the user uses the filter box on the homescreen.

When a filter box is ticked, only schemes that have the corresponding filter aspect associated with it are included in the result set.

The ordering of the filter aspectes associated with a scheme is of no import.

#### Case studies (edit box)

The old way of entering case studies content. Has been replaced by adding case study references, and this field will eventually be removed.

#### Case studies (references)

The case studies associated with the scheme, which are rendered as part of the scheme details page.

If a single case study is associated with a scheme, it is rendered inline. If more than one case study is associated with a scheme, the case studies are rendered in an accordion.

The rendering order is determined by the order on the scheme edit page.

### Case study

Case studies that are referenced and rendered as part of the scheme details page.

### Motivations, Pay and Scheme Length filter

The content items of these filter aspect content types determine what filter options are presented in the filter box, and when associated with schemes, determing the filter result sets.

The description field is used for the checkbox text in the filter box.

The order field determines the order the filter aspects are displayed in the filter box.

### Page

Pages can be created, edited and deleted, and the web site will update accordingly.

Pages are located at https://find-employer-schemes.education.gov.uk/page/{page-url}, where `{page-url}` is taken from the Url field of the page.

#### Error injection page

There is a special [error injection page](https://find-employer-schemes.education.gov.uk/page/error-check), that allows us to check the error page.

### Case study page

Case study pages can be created, edited and deleted, and the web site will update accordingly.

Case study pages are located at https://find-employer-schemes.education.gov.uk/case-study/{page-url}, where `{page-url}` is taken from the Url field of the page.

## Publish and Preview

Production content is edited in a single environment. (See below for details on environments, but content editors will see only see the single 'master' production environment.)

Draft pages and schemes can be previewed in the production environment by clicking the 'Open preview' button. That will open a browser tab showing the draft scheme or page (including draft versions of any referenced content types, such as draft filters).

Once draft content is finalised and has had a second person review and approve it, the item can be published.

The web site updates on a schedule. The current schedule for production is for updates to occur every half hour from 6:00 to 23:30. (The schedule is configurable and can be changed on request.)

That means e.g. if you publish content at 9:15, it will appear on the web site at 9:30.

It also means that if you have a set of content updates that you want to be published together, you can publish them all within the window and they will all be published simultaneously.

## Sitemap

The [sitemap](https://find-employer-schemes.education.gov.uk/sitemap.xml) is updated automatically (on schedule) when pages, schemes and case study pages are published or unpublished.

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