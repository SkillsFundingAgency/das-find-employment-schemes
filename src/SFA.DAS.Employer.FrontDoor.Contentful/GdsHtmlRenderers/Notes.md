# Notes

We shouldn't need a GdsTextRenderer, as only bold is supported in GDS (italics and underline are not), and `<strong>` is fine (`govuk-!-font-weight-bold` is only required on other elements).

# todo

contentful wrapps li's in <p> - so we'll have to strip them out

contentful's <p> handling is inconsistent - is there a way to switch the rich text box to show the underlying html, like orchard core supports?
