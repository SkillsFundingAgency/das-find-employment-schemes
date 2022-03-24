# User Guide

This guide explains how to update the [Find Employer Schemes](https://find-employer-schemes.education.gov.uk/) (FES) website, by editing the content in the Content Management System (CMS). This is primarily a guide for content authors/approvers, but will also be useful for developers and testers.

## Contentful CMS

[Contentful](https://www.contentful.com) is the CMS used to edit the content for the website. Log in to Contentful with your given credentials.

## 2i and roles

The content team are given both the 'author' and 'editor' roles. This means every team member can both edit and publish content. It is up to the team, to ensure that all content edits are reviewed by someone other than the author, before publishing.

Developers and testers are given the administrator role, so that they can edit the schema and make use of the test and development environments (see [Environments](#environments)).

## Editing

For a general guide to Contentful, consult the [Contentful help center](https://www.contentful.com/help/).

Some customisations of the out of the box editing experience have been made and are detailed here:

#### Call to action box

As there is no formatting support in Contentful for a call to action box, we instead mark the content we want to appear in a call to action box in a blockquote, with a given first line of `<cta>`.

#### Links

Links can be added in the normal way for Contentful.

If you need a link to open in a new tab, append `(opens in new tab)` to the end of the link text (as recommended by GDS), and the web site will render the link so that it opens a new tab, if clicked.

#### Embedded YouTube videos

If you want to embed a YouTube video, copy the embed code for the YouTube video, making sure to select "Enable privacy-enhanced mode", and copy that into an edit box. Here's a [guide to get the embed code](https://axbom.com/embed-youtube-videos-without-cookies/).

Enabling the privacy-enhanced mode makes the embed code reference `youtube-nocookie.com`, which stops YouTube setting cookies on the site. If you forget to enable privacy-enhanced mode, the website will automatically convert the domain for you, but you should get in the habbit of enabling the mode, in case YouTube changes how it works.

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

Schemes detail pages are located at https://find-employer-schemes.education.gov.uk/schemes/{page-url}, where `{page-url}` is taken from the Url field of the scheme.

The editor has help text for each field, but some fields require more explanation, which are given here:

##### Details page override

Any content in the 'Details Page Override' field overrides the usual composition of the scheme details page, from the normal set of fields, such as 'Description' and 'Cost'.

Instead the main content of the scheme details page is taken from this field. It can be used when the required content for the scheme doesn't match the usual format.

No current scheme uses the details page override, it was introduced for a scheme that's now been replaced for one that uses sub schemes, but the facility remains in case it is needed.

##### Sub schemes

If any sub schemes are added to a scheme, they are rendered in an accordion on the details page below the description. The other usual scheme detail headers (such as cost and benefit) are not rendered if a scheme references a sub scheme.

The website rendering order, is determined by the order of the referenced sub schemes on the scheme edit page.

See the 'Free courses and additional training for your employees' scheme as an example.

##### Additional footer

Additional footer content to render in the footer, _above_ the usual footer content. See the Apprenticeship scheme for an example.

##### Size

The size field determines the ordering of the schemes on the home page and in the other schemes section on the scheme details pages. The schemes with the bigger sizes are rendered higher up.

The existing schemes sizes have been set to the number of citizens currently on the scheme, but the size field can be arbitrary.

##### Filter aspects

The Motivations, Pay and Scheme Length filter aspects referenced by a scheme determine whether the scheme is part of the result set, when the user uses the filter box on the homepage.

When a filter box is ticked, only schemes that have the corresponding filter aspect associated with it are included in the result set.

The ordering of the filter aspects associated with a scheme is of no import.

##### Case studies (edit box)

Case study content.

If both case study content is supplied _and_ case study items are referenced, this content is rendered above the referenced case studies, as a preamble.

##### Case studies (references)

The case studies associated with the scheme, which are rendered as part of the scheme details page.

If a single case study is associated with a scheme, it is rendered inline. If more than one case study is associated with a scheme, the case studies are rendered using the GDS details component.

The website rendering order, is determined by the order of the referenced case study items on the scheme edit page.

### Sub scheme

Blocks of content that can be referenced and rendered as part of the scheme details page.

The sub schemes are rendered in an accordion below the scheme description.

### Case study

Case studies that are referenced and rendered as part of the scheme details page.

### Motivations, Pay and Scheme Length filter

The content items of these filter aspect content types determine what filter options are presented in the filter box and when associated with schemes, determine the filter result sets.

The description field is used for the checkbox text in the filter box.

The order field determines the order the filter aspects are displayed in the filter box.

### Page

Pages can be created, edited and deleted, and the web site will update accordingly.

Pages are located at https://find-employer-schemes.education.gov.uk/page/{page-url}, where `{page-url}` is taken from the Url field of the page.

> Note: There is a special [error injection page](https://find-employer-schemes.education.gov.uk/page/error-check), that allows us to check the error page.

### Case study page

Case study pages can be created, edited and deleted, and the web site will update accordingly.

Case study pages are located at https://find-employer-schemes.education.gov.uk/case-study/{page-url}, where `{page-url}` is taken from the Url field of the case study page.

## Publish and Preview

Production content is edited in a single environment (see [Environments](#environments)). Content editors don't see the option to switch environments, they only see the single 'master' production environment.

Draft pages, schemes and case study pages can be previewed in the production environment by clicking the 'Open preview' button. That opens a browser tab showing the draft scheme, page or case study page (including draft versions of any referenced content types, such as draft filters).

When a preview page is being displayed, the site is in preview mode. All links will keep the site in preview mode. If a draft version of content is available, that will be shown, otherwise the published version will be used.

All preview pages have `preview/` prepended to the URL path. The home page of the preview site can be accessed using `https://find-employer-schemes.education.gov.uk/preview`.

Once draft content is finalised and has had a second person review and approve it, the item can be published.

The web site updates on a schedule. The current schedule for production is for updates to occur every half hour from 6:00 to 23:30. (The schedule is configurable and can be changed on request.)

That means e.g. if you publish content at 9:15, it will appear on the web site at 9:30.

It also means that if you have a set of content updates that you want to be published together, you can publish them all within the window and they will all be published simultaneously.

## Sitemap

The [sitemap](https://find-employer-schemes.education.gov.uk/sitemap.xml) is updated automatically (on schedule) when pages, schemes and case study pages are published or unpublished.

## Environments

Developers and testers will be given the 'administrator' role. This allows the use of different Contentful environments.

When developing and testing new features, schema and content changes can be made, without having to touch the 'master' production environment. E.g. when schema or content changes are made in the Contentful test environment, the changes are visible in the FES test (and test2) environment.

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

The pp environment updates on the same schedule as production. The other environments update every half hour at 15 and 45 past the hour, from 7:15 to 18:45.

The updates on the other environments are staggered compared to prod and pp, so that we don’t hit any Contentful API request throttling issues.