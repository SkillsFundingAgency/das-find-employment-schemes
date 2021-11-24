using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using SFA.DAS.Employer.FrontDoor.Web.Models;

namespace SFA.DAS.Employer.FrontDoor.Web.Content
{
    public static class SchemesContent
    {
        public static readonly IEnumerable<Scheme> Schemes = new[]
            {
                // ensure we order by size desc, so we don't have to sort

new Scheme("Apprenticeships",
new HtmlString(@"<p>Paid employment for over 16s, combining work and study in a job allowing you to develop your workforce and business.</p>"),
new HtmlString(@"<p>Apprentice minimum wage and 5% training contribution depending on business size</p>"),
new HtmlString(@"<p>You develop a motivated, skilled, and qualified workforce</p>"),
new HtmlString(@"<p>Minimum of 12 months employment</p>"),
"apprenticeships", 403000, null,
new HtmlString(@"<p>Applies to England</p><p>Apprenticeships are for those aged 16 or over and combine working with studying to gain skills and knowledge in a job role.</p><p>Apprentices can be new or current employees.</p><p>Your apprentice must:</p><ul class =""govuk-list govuk-list--bullet""><li><p>work with experienced staff</p></li><li><p>learn skills relevant to your organisation</p></li><li><p>get time for training or study during their working week (at least 20% of their normal working hours)</p></li></ul><p></p>"),
new HtmlString(@"<p>You must pay apprentices <a href=""https://www.gov.uk/national-minimum-wage-rates"" title="""" class=""govuk-link"">the minimum wage</a>.</p><p>You may have to contribute 5% of the apprentice's training and assessment costs - depending on the size of your business and the age of your apprentice.</p>"),
new HtmlString(@"<p>Employers must provide:</p><ul class =""govuk-list govuk-list--bullet""><li><p>a safe work environment</p></li><li><p>an induction which includes explaining relevant policies and procedures</p></li><li><p>opportunities for the student to develop their technical skills within your industry</p></li><li><p>20% 'Off-the-job' training delivered by a training provider during your apprentice&#39;s normal working hours</p></li><li><p>'On-the-job' training and additional support to ensure your apprentice's success</p></li></ul><p>Employers must also have:</p><ul class =""govuk-list govuk-list--bullet""><li><p>a signed apprenticeship agreement and apprenticeship commitment statement from the learner and training provider</p></li></ul><p></p>"),
new HtmlString(@"<p>Find out <a href=""https://www.apprenticeships.gov.uk/"" title="""" class=""govuk-link"">how apprenticeships can benefit your business</a>.</p>"),
new HtmlString(@"<p>Read about <a href=""https://www.apprenticeships.gov.uk/employers/real-stories-employers"" title="""" class=""govuk-link"">how businesses are using incentive payments to hire apprentices</a>.</p>"),
"Offer an apprenticeship",
new HtmlString(@"<p>Get started with offering an apprenticeship.</p><p><a href=""https://www.apprenticeships.gov.uk/employers/hiring-an-apprentice"" title="""" class=""govuk-link"">Hire an apprentice</a></p><p>Or call <a href=""tel:08000150400"" title="""" class=""govuk-link"">08000 150 400</a></p>")),
new Scheme("T Levels: industry placements",
new HtmlString(@"<p>Provide a 45 day (315 hours) industry placement for 16 to 19 year-olds, which gives you early access to the brightest entering the market and the opportunity to develop your workforce of the future.</p>"),
new HtmlString(@"<p>Free government scheme but you may have your own business costs</p>"),
new HtmlString(@"<p>Industry placements can save on many of the costs associated with recruitment</p>"),
new HtmlString(@"<p>Short term 45-day industry placement</p>"),
"t-levels-industry-placements", 72000, null,
new HtmlString(@"<p>T Levels are qualifications for students aged 16 to 19 who have finished GCSEs. They are a 2-year qualification and the equivalent to 3 A levels.</p><p>They have been designed with employers, to give them the skilled workforce they need by helping young people develop their knowledge, attitude and practical skills to thrive in the workplace.</p>"),
new HtmlString(@"<p>Free government scheme but you may have your own business costs.</p><p>Some employers may choose to pay wages and cover expenses such as travel and living costs.</p>"),
new HtmlString(@"<p>Employers must provide:</p><ul class =""govuk-list govuk-list--bullet""><li><p>a safe work environment</p></li><li><p>opportunities for the student to develop their technical skills within your industry</p></li><li><p>a line manager to support, supervise and mentor the student</p></li><li><p>an induction which includes explaining relevant policies and procedures</p></li><li><p>formal feedback on the student's progress against the agreed learning goals at the end of the placement</p></li></ul><p></p>"),
new HtmlString(@"<p>T Levels are available in more than 20 courses, covering 11 skill areas.</p><p>T Levels can support:</p><ul class =""govuk-list govuk-list--bullet""><li><p>you to build a pipeline of entry-level positions</p></li><li><p>the next generation of workers entering your industry to succeed</p></li><li><p>you with recruitment, improve innovation and increase your organisation's productivity</p></li><li><p>you to fill skills shortages&#160;</p></li><li><p>your local community and improve diversity</p></li><li><p>your existing staff gain management and mentoring skills</p></li></ul><p>Find out <a href=""https://www.tlevels.gov.uk/employers"" title="""" class=""govuk-link"">how T Levels can benefit your business</a>.

</p>"),
new HtmlString(@"<p>Read about <a href=""https://www.gov.uk/government/case-studies/how-employers-are-benefitting-from-t-level-industry-placements"" title="""" class=""govuk-link"">how businesses are using T Levels industry placements</a>.</p>"),
"Offer an industry placement",
new HtmlString(@"<p>Get started with offering an industry placement.</p><p><a href=""https://employers.tlevels.gov.uk/hc/en-gb/requests/new"" title="""" class=""govuk-link"">Take the next steps with T Levels</a></p><p>Or call <a href=""tel:08000150600"" title="""" class=""govuk-link"">08000 150 600</a> (choose option 4)</p>")),
new Scheme("Sector-based Work Academy Programme (SWAP)",
new HtmlString(@"<p>Up to 6-week placement for benefit claimants aged 18+ designed to help you recruit a workforce with the right skills to sustain and grow your business.</p>"),
new HtmlString(@"<p>Free government scheme but you may have your own business costs</p>"),
new HtmlString(@"<p>Helps you recruit or train staff through fully-funded pre-employment training</p>"),
new HtmlString(@"<p>Up to 6-weeks</p>"),
"sector-based-work-academy-programme-swap", 70000, null,
new HtmlString(@"<p>Sector-based Work Academy Programmes (SWAPs) help prepare those receiving unemployment benefits to apply for jobs in a different area of work.</p><p>Placements run up to 6 weeks and are designed to help you recruit a workforce with the right skills to sustain and grow your business.</p><p>They are tailored to meet your recruitment needs, including pre-employment training, a work experience placement, and your guarantee of a job interview for participants.</p><p>Jobcentre Plus manages SWAPs in England and Scotland.</p>"),
new HtmlString(@"<p>Free government scheme but you may have your own business costs.</p>"),
new HtmlString(@"<p>In order for both businesses and individuals to benefit from the work experience placement, Jobcentre Plus will ask that you:</p><ul class =""govuk-list govuk-list--bullet""><li><p>explain what you need the scheme's participants to do</p></li><li><p>inform them of management and reporting arrangements</p></li><li><p>encourage positive working relationships</p></li><li><p>provide an overview of your business and its values and culture</p></li><li><p>give a tour of the workplace</p></li><li><p>provide guidance on using any equipment</p></li><li><p>provide guidance on health and safety</p></li><li><p>explain standard workplace practices such as security procedures, dress code, sick leave and absences and break times</p></li></ul><p></p>"),
new HtmlString(@"<p>Find out <a href=""https://www.gov.uk/government/publications/sector-based-work-academies-employer-guide/sector-based-work-academies-employer-guide"" title="""" class=""govuk-link"">how SWAP could benefit your business</a>.</p>"),
new HtmlString(@"<p>Read about <a href=""https://www.gov.uk/government/case-studies/how-employers-are-benefitting-from-sector-based-work-academies"" title="""" class=""govuk-link"">how employers are using sector-based work academy programmes</a>.</p>"),
"Offer a SWAP",
new HtmlString(@"<p>Contact the Employer Services Line on <a href=""tel:08001690178"" title="""" class=""govuk-link"">0800 169 0178</a></p><p>They will put you in touch with a local Jobcentre Plus employer team.</p>")),
new Scheme("Skills Bootcamps",
new HtmlString(@"<p>Flexible work and training courses for over 19's to fast-track specialist skill development, for existing or new talent for your business.</p>"),
new HtmlString(@"<p>No cost unless self-employed</p>"),
new HtmlString(@"<p>Helps future-proof your business in a rapidly changing labour market and recruit staff with the right training and skills from the outset</p>"),
new HtmlString(@"<p>12 to 16-weeks</p>"),
"skills-bootcamps", 68000, null,
new HtmlString(@"<p>Skills Bootcamps offer free, flexible courses of up to 16 weeks for adults aged 19 or over and who are either in work, self-employed, recently unemployed or returning to work after a break.</p><p>They give people the opportunity to build up valuable sector-specific skills based on local employer demand in areas including digital, construction and green skills.</p><p>They provide a direct path to a job on completion.</p>"),
new HtmlString(@"<p>Employers who want to use Skills Bootcamps for their existing employees contribute 30% of the cost.</p><p>Existing employees may need time to study and attend classes depending on their working hours and status and the flexibility of the course.</p>"),
new HtmlString(@"<p>Employers must provide:</p><ul class =""govuk-list govuk-list--bullet""><li><p>a safe work environment</p></li><li><p>an induction which includes explaining relevant policies and procedures</p></li><li><p>a line manager to support, supervise and mentor the student</p></li><li><p>a guaranteed interview as part of the Skills Bootcamp provision</p></li></ul><p></p>"),
new HtmlString(@"<p>They help you:</p><ul class =""govuk-list govuk-list--bullet""><li><p>develop a loyal and talented workforce with the skills you need</p></li><li><p>futureproof your business in a rapidly changing labour market</p></li><li><p>improve your productivity; learners are motivated to learn new skills, provide new ideas and a fresh perspective</p></li><li><p>recruit staff with the right training and skills from the outset</p></li><li><p>get access to a pipeline of skilled adults at no cost by working with existing Skills Bootcamp providers </p></li></ul><p></p>"),
null,
"Offer a Skills Bootcamps",
new HtmlString(@"<p>Get started with offering Skills Bootcamps and find providers offering courses in your area.</p><p><a href=""https://www.gov.uk/government/publications/find-a-skills-bootcamp"" title="""" class=""govuk-link"">Find Skills Bootcamps in your area</a></p>")),
new Scheme("Traineeships",
new HtmlString(@"<p>A 6-week to 12 months skills development programme that includes an unpaid work placement to help 16- to 24-year-olds prepare for an apprenticeship or employment.</p>"),
new HtmlString(@"<p>Free government scheme but you may have your own business costs</p>"),
new HtmlString(@"<p>Helps you increase your capacity and productivity whilst developing a loyal and talented workforce</p>"),
new HtmlString(@"<p>6-week to 12 months</p>"),
"traineeships", 43000, null,
new HtmlString(@"<p>A traineeship is a skills development programme that includes a work placement.</p><p>The full programme can last from 6 weeks up to 1 year, though most traineeships last for less than 6 months.</p><p>Your business needs to be able to offer at least 70 hours of a safe, meaningful, and high-quality work experience. You will work with a training provider to design the programme.</p><p>The trainee will gain English, maths, digital and work-related qualifications which can lead them on to:</p><ul class =""govuk-list govuk-list--bullet""><li><p>an apprenticeship</p></li><li><p>work</p></li><li><p>further education</p></li></ul><p></p>"),
new HtmlString(@"<p>Free government scheme but you may have your own business costs.</p><p>Some employers may choose to pay the wages and any payments for expenses such as travel and living costs.
</p>"),
new HtmlString(@"<p>Employers must provide:</p><ul class =""govuk-list govuk-list--bullet""><li><p>a safe, meaningful and high-quality work experience placement</p></li><li><p>a minimum of 70 hours of work experience placement over the duration of the traineeship (if the trainee claims benefits, the placement cannot last longer than 240 hours)</p></li><li><p>constructive feedback and advice to the trainee</p></li><li><p>an interview for an apprenticeship or job in their organisation at the end of the traineeship if one is available</p></li><li><p>an exit interview at the end of the traineeship with meaningful written feedback if no job is available</p></li></ul><p></p>"),
new HtmlString(@"<p>Traineeships have seen big increases as businesses recover from the pandemic and are expected to double to around 40,000 per year.</p><p>Find out <a href=""https://www.gov.uk/guidance/traineeship-information-for-employers"" title="""" class=""govuk-link"">how traineeships can benefit your business</a>.</p>"),
new HtmlString(@"<p>Read about <a href=""https://www.gov.uk/government/case-studies/how-employers-are-benefitting-from-traineeships"" title="""" class=""govuk-link"">how businesses are using traineeships</a>.</p>"),
"Offer a traineeship",
new HtmlString(@"<p>Get started with offering a traineeship.</p><p><a href=""https://www.gov.uk/guidance/traineeship-information-for-employers#get-started"" title="""" class=""govuk-link"">Contact the National Apprenticeship Service</a></p><p>Or call <a href=""tel:08000150600"" title="""" class=""govuk-link"">08000 150 600</a></p>")),
new Scheme("Supported Internships for learners with an education, health and care plan",
new HtmlString(@"<p>An unpaid 6 to 12-month work placement for 16 to 24-year-olds with additional needs supported by a qualified job coach.</p>"),
new HtmlString(@"<p>Free government scheme but you may have your own business costs.</p>"),
new HtmlString(@"<p>Allows you to shape a job role that suits your business and the needs of the supported intern and increases your confidence of employing individuals with additional needs</p>"),
new HtmlString(@"<p>6 to 12-months</p>"),
"supported-internships-for-learners-with-an-education-health-and-care-plan", 20000, null,
new HtmlString(@"<p>Supported internships are a work-based study programme for young people aged 16 to 24 with Special Educational Needs and Disabilities (SEND), who have an <a href=""https://www.gov.uk/children-with-special-educational-needs/extra-SEN-help"" title="""" class=""govuk-link"">education health and care (EHC) plan</a>.</p><p>Supported internships equip young people with the skills they need to secure and sustain employment through learning in the workplace, with support from a qualified job coach. They last between 6 to 12 months.</p><p>Supported interns are enrolled with and supported by a learning provider, but spend most of their learning time - typically around 70% in the workplace.</p><p>Alongside their time with the employer, supported interns complete a personalised study programme delivered by the school/college, which includes the chance to study for relevant qualifications, if appropriate, and English and maths at an appropriate level.</p>"),
new HtmlString(@"<p>Free government scheme but you may have your own business costs.</p><p>Some employers may choose to pay wages and cover expenses such as travel and living costs.</p>"),
new HtmlString(@"<p>Employers must provide:</p><ul class =""govuk-list govuk-list--bullet""><li><p>a safe work environment</p></li><li><p>workplaces adjustments the supported intern may need</p></li><li><p>a line manager to support, supervise and mentor the student</p></li><li><p>an induction which includes explaining relevant policies and procedures</p></li><li><p>a welcoming environment and be willing to work collaboratively with the job coach, to support the young person to succeed</p></li></ul><p></p>"),
new HtmlString(@"<p>Supported internships:</p><ul class =""govuk-list govuk-list--bullet""><li><p>create and support a diverse workforce</p></li><li><p>provide a job coach who will support the intern and increase your confidence of employing individuals with additional needs</p></li><li><p>increase your capacity and productivity</p></li><li><p>allows you to shape a job role that suits both the needs of your business and the needs of the supported intern</p></li></ul><p></p>"),
null,
"Offer a supported internship",
new HtmlString(@"<p>Offer a Supported Internship by contacting the lead for your region from the <a href=""https://www.preparingforadulthood.org.uk/about-us/meet-the-team.htm#Regional%20Leads"" title="""" class=""govuk-link"">Preparing for Adulthood Team</a></p>")),
new Scheme("Care-Leaver covenant",
new HtmlString(@"<p>Help 16 to 25-year-olds who were in local authority care become independent through practical job-related support, in whatever way suits your business.</p>"),
new HtmlString(@"<p>Free government scheme but you may have your own business costs related to the practical offer you make to care leavers</p>"),
new HtmlString(@"<p>Helping care-leavers to move to independent living and bring their life skills to enrich your business</p>"),
new HtmlString(@"<p>Length of time can vary</p>"),
"care-leaver-covenant", 2000, null,
new HtmlString(@"<p>The Care-leaver Covenant is a way companies can support 16 to 25-year-olds leaving care to become independent.</p><p>Organisations can pledge support including apprenticeships, work experience or free or discounted goods or services. About the CLC - Care Leaver Covenant (mycovenant.org.uk)</p>"),
new HtmlString(@"<p>You may have your own business costs related to the practical offer your business makes to care leavers.</p>"),
new HtmlString(@"<p>Employers must provide:</p><ul class =""govuk-list govuk-list--bullet""><li><p>a safe work environment</p></li><li><p>an induction which includes explaining relevant policies and procedures</p></li><li><p>practical help that makes a difference to the life of a care leaver</p></li></ul><p></p>"),
new HtmlString(@"<p>You create and support a diverse and loyal workforce for your business.</p>"),
new HtmlString(@"<p>Read how <a href=""https://mycovenant.org.uk/covenant-stories/"" title="""" class=""govuk-link"">companies have been making a difference to the lives of care leavers</a></p>"),
"Offer a Care Leavers Covenant",
new HtmlString(@"<p><a href=""https://mycovenant.org.uk/support-the-covenant/organisation-sign-up/"" title="""" class=""govuk-link"">Find out how to join the Care Leavers Covenant</a> and tailor what opportunities your business would like to offer to care leavers.</p><p>Alternatively, contact the Care Leavers Covenant via telephone on <a href=""tel:08000773557"" title="""" class=""govuk-link"">0800 077 3557</a>.</p>")),
new Scheme("Employing prisoners and prison leavers",
new HtmlString(@"<p>Employ prisoners and prison leavers aged 18+ to help your business fill skills gaps and develop a loyal and talented workforce.</p>"),
new HtmlString(@"<p>Free government scheme but you may have your own business costs</p>"),
new HtmlString(@"<p>Helping prisoners and prison leavers find employment</p>"),
new HtmlString(@"<p>Variable depending on employer and prisoner and prison leaver</p>"),
"employing-prisoners-and-prison-leavers", 1000, null,
new HtmlString(@"<p>New Futures Network (NFN) arranges partnerships between prisons and employers in England and Wales. These partnerships help you:</p><ul class =""govuk-list govuk-list--bullet""><li><p>fill skills gaps in your business</p></li><li><p>reduce recruitment costs</p></li><li><p>increase staff retention</p></li><li><p>transform prison leavers lives</p></li></ul><p></p>"),
new HtmlString(@"<p>Free government scheme but you may have your own business costs. If you recruit someone you must pay <a href=""https://www.gov.uk/national-minimum-wage-rates"" title="""" class=""govuk-link"">the minimum wage</a>.</p>"),
new HtmlString(@"<p>Employers must provide:</p><ul class =""govuk-list govuk-list--bullet""><li><p>a safe work environment</p></li><li><p>a line manager to support, supervise and mentor</p></li><li><p>an induction which includes explaining relevant policies and procedures</p></li></ul><p></p>"),
new HtmlString(@"<p>Over 400 businesses already work in partnership with prisons to provide work and employment opportunities.</p><p>Of those surveyed, more than 80% of employers positively rated those they employed as reliable and hard-working.</p>"),
new HtmlString(@"<p>Read about <a href=""https://newfuturesnetwork.gov.uk/case-studies/#"" title="""" class=""govuk-link"">how businesses are working with serving prisoners and hiring prison leavers</a>.</p>"),
"Offer prisoners and prison leavers employment",
new HtmlString(@"<p><a href=""https://newfuturesnetwork.gov.uk/register/"" title="""" class=""govuk-link"">Register your interest on the New Futures Network</a>.</p><p>A member of the team will be in touch to discuss the process.</p>")),
new Scheme("Training outside of employment",
new HtmlString(@"<p>Additional ways to train up existing employees through free qualifications, career advice and financial support.</p>"),
new HtmlString(@"<p>Courses are fully funded for eligible adults but employees may need time to study and attend classes</p>"),
new HtmlString(@"<p>Help improve productivity, and develop a loyal and talented workforce in a rapidly changing labour market</p>"),
new HtmlString(@"<p>Length of time can vary</p>"),
"training-outside-of-employment", -1000, new HtmlString(@"<p>Career advice, free level 3 qualifications, and financial support for level 4 and 5 qualifications are all available through Government initiatives for your workforce.</p><h2 class=""govuk-heading-l"">Free qualifications for adults</h2><p><a href=""https://www.gov.uk/guidance/free-courses-for-jobs"" title="""" class=""govuk-link"">Free level 3 qualifications</a> are government-funded courses for any adult aged 19 and over, who are looking to achieve their first full level 3 qualification or earning below national minimum wage.</p><p>A full level 3 qualification is equivalent to an advanced technical certificate, diploma, or A levels.</p><p>Without needing to fund the training yourself, they help you:</p><ul class =""govuk-list govuk-list--bullet""><li><p>develop the talent pool in your business</p></li><li><p>progress your current employees into higher skilled roles</p></li></ul><p>The <a href=""https://www.gov.uk/guidance/free-courses-for-jobs"" title="""" class=""govuk-link"">Free Courses for jobs</a> offer and the qualification list has been developed with industry, and will be regularly reviewed. </p><h3 class=""govuk-heading-m"">Benefits of free level 3 qualifications</h3><p>Read the <a href=""https://learning.linkedin.com/content/dam/me/business/en-us/amp/learning-solutions/images/workplace-learning-report-2019/pdf/workplace-learning-report-2019.pdf"" title="""" class=""govuk-link"">LinkedIn Workplace Learning</a> report 2019 for information on developing a loyal and talented workforce.</p><h3 class=""govuk-heading-m"">Offer free qualifications to adults</h3><p>Upskill your workforce by helping your employees <a href=""https://www.gov.uk/government/publications/find-a-free-level-3-qualification"" title="""" class=""govuk-link"">find a free level 3 qualification</a>.</p><h2 class=""govuk-heading-l"">National Careers Service</h2><p>The National Careers Service can help you:</p><ul class =""govuk-list govuk-list--bullet""><li><p>explore work and skills opportunities for your workforce including those offered as part of the skills recovery package.</p></li><li><p>carry out skills needs analysis for your business to understand gaps and find solutions.</p></li><li><p>find skilled people to fill current vacancies in your organisation.</p></li></ul><h3 class=""govuk-heading-m"">Employer costs</h3><p>The National Careers Service is free to use.</p><h3 class=""govuk-heading-m"">Benefits of National Careers Service</h3><p>The National Careers Service can help you:</p><ul class =""govuk-list govuk-list--bullet""><li><p>futureproof your business for the future labour market</p></li><li><p>help to develop your current workforce and supplement with talented individuals who have the skills you need</p></li><li><p>increase your resilience and productivity</p></li><li><p>take part in national virtual jobs fairs</p></li></ul><h3 class=""govuk-heading-m"">Use the National Careers Service to support your workforce</h3><p>Find out more about the <a href=""https://nationalcareers.service.gov.uk/"" title="""" class=""govuk-link"">National Careers Service and how to contact them</a>.</p><p></p>"),
null,
null,
null,
null,
null,
"",
null)

  //              new Scheme("Apprenticeships",
  //                  new HtmlString("Paid employment for over 16s combining work and study in a specific job allowing you to develop your workforce and business."),
  //                  new HtmlString("Apprentice minimum wage and 5% training contribution depending on business size"),
  //                  new HtmlString("You develop a motivated, skilled and qualified workforce"),
  //                  new HtmlString("Minimum of 12 months employment"),
  //                  "apprenticeships", 1000, new HtmlString(""),
  //                  new HtmlString(@"<p class=""govuk-body"">
		//	Applies to England
		//</p>
		//<p class=""govuk-body"">
		//	<a href=""https://www.apprenticeships.gov.uk/employers"" class=""govuk-link"">Apprenticeships</a> are for those aged 16 or over and combine working with studying to gain skills and knowledge in a job role.
		//</p>
		//<p class=""govuk-body"">
		//	<a href=""https://www.gov.uk/apprenticeships-guide"" class=""govuk-link""> Apprentices</a> can be new or current employees. 
		//</p>
		//<p class=""govuk-body"">
		//	Your apprentice must: 
		//</p>
		//<ul class=""govuk-list govuk-list--bullet"">
		//	<li>work with experienced staff</li>
		//	<li>learn skills relevant to your organisation</li>
		//	<li>get time for training or study during their working week (at least 20% of their normal working hours)</li>
		//</ul>"),
  //              new HtmlString(@"<p class=""govuk-body"">
  //          You must pay apprentices <a href=""https://www.gov.uk/national-minimum-wage-rates"" class=""govuk-link"">the minimum wage</a>.
		//</p>
		//<p class=""govuk-body"">
  //          You may have to contribute 5% of the apprentice’s training and assessment costs – depending on the size of your business and the age of your apprentice.
		//</p>"),
  //              new HtmlString(@"<p class=""govuk-body"">
		//	Employers must provide:
		//</p>
		//<p class=""govuk-body"">
		//</p>
		//<ul class=""govuk-list govuk-list--bullet"">
		//	<li>a safe work environment</li>
		//	<li>an induction which includes explaining relevant policies and procedures</li>
		//	<li>opportunities for the student to develop their technical skills within your industry</li>
		//	<li>20% ‘Off-the-job’ training delivered by a training provider during your apprentice's normal working hours</li>
		//	<li>‘On-the-job’ training and additional support to ensure your apprentice’s success</li>
		//</ul>
		//<p class=""govuk-body"">
		//	Employers must also have:
		//</p>
		//<p class=""govuk-body"">
		//</p>
		//<ul class=""govuk-list govuk-list--bullet"">
		//	<li>a signed apprenticeship agreement and apprenticeship commitment statement from the learner and training provider</li>
		//</ul>"),
  //                  new HtmlString(@"<p class=""govuk-body"">
		//	Find out <a href=""https://www.apprenticeships.gov.uk/"" class=""govuk-link"">how apprenticeships can benefit your business</a>.
		//</p>"),
  //                  new HtmlString(@"<p class=""govuk-body"">
		//	Read about <a href=""https://www.gov.uk/government/case-studies/how-employers-are-making-the-most-of-apprenticeship-incentives"" class=""govuk-link"">how businesses are using incentive payments to hire apprentices</a>.
		//</p>"),
  //                  "Offer an apprenticeship",
  //                  //todo: we won't be able to add "govuk-!-margin-bottom-0" on the last <p> from contentful
  //                  // so we'll have to replace das-highlight with a cx-cta-box or similar with different bottom padding
  //                  new HtmlString(@"<p class=""govuk-body"">
		//		Get started with offering an apprenticeship.
		//	</p>
		//	<p class=""govuk-body"">
		//		<a href=""https://www.apprenticeships.gov.uk/employers"" class=""govuk-link""> Hire an apprentice </a>
		//	</p>
  //          <p class=""govuk-body"">
		//		Or call <a href=""tel:08000150400"" class=""govuk-link"">08000 150 400</a>
  //          </p>")),
  //              new Scheme("T Levels: industry placements",
  //                  new HtmlString("Provide a 45 day (315 hours) industry placement for 16 to 19 year-olds, which gives you early access to the brightest entering the market and the opportunity to develop your workforce of the future."),
  //                  new HtmlString("Free government scheme but you may have your own business costs"),
  //                  new HtmlString("Industry placements can save on many of the costs associated with recruitment"),
  //                  new HtmlString("Short term 45-day industry placement"),
  //                  "t-levels", 900, new HtmlString(""),
  //                  new HtmlString(@"<p class=""govuk-body"">
		//		        <a href=""https://www.tlevels.gov.uk/employers"" class=""govuk-link"">T Levels</a> are qualifications for students aged 16 to 19 who have finished GCSEs. They are a 2-year qualification and the equivalent to 3 A levels.
  //                  </p>
  //                  <p class=""govuk-body"">
  //                      They have been designed with employers, to give them the skilled workforce they need by helping young people develop their knowledge, attitude and practical skills to thrive in the workplace.
  //                  </p>"),
  //                  new HtmlString(@"<p class=""govuk-body"">
  //                  Free government scheme but you may have your own business costs.
  //                  </p>
  //                  <p class=""govuk-body"">
  //                  Some employers may choose to pay wages and cover expenses such as travel and living costs.
  //                  </p>"),
  //                  new HtmlString(@""),
  //                  new HtmlString(@""),
  //                  new HtmlString(@""),
  //                  "",
  //                  new HtmlString(@"")),
  //              new Scheme("Traineeships",
  //                  new HtmlString("A 6-week to 12 months skills development programme that includes an unpaid work placement to help 16- to 24-year-olds prepare for an apprenticeship or employment."),
  //                  new HtmlString("Free government scheme but you may have your own business costs"),
  //                  new HtmlString("Helps you increase your capacity and productivity whilst developing a loyal and talented workforce"),
  //                  new HtmlString("6-week to 12 months"),
  //                  "traineeships", 800),
  //              new Scheme("Skills Bootcamps",
  //                  new HtmlString("Flexible work and training courses for over 19’s to fast-track specialist skill development, for existing or new talent for your business."),
  //                  new HtmlString("No cost unless self-employed"),
  //                  new HtmlString("Helps future-proof your business in a rapidly changing labour market and recruit staff with the right training and skills from the outset"),
  //                  new HtmlString("12 to 16-weeks"),
  //                  "skills-bootcamps", 700),
  //                  new Scheme("Sector-based Work Academy Programme (SWAP)",
  //                  new HtmlString("Up to 6-week placement for benefit claimants aged 18+ designed to help you recruit a workforce with the right skills to sustain and grow your business."),
  //                  new HtmlString("Free government scheme but you may have your own business costs"),
  //                  new HtmlString("Helps you recruit or train staff through fully-funded pre-employment training"),
  //                  new HtmlString("Up to 6-weeks"),
  //                  "swap", 600),
  //              new Scheme("Supported Internships for learners with an education, health and care plan",
  //                  new HtmlString("An unpaid 6 to 12-month work placement for 16 to 24-year-olds with additional needs supported by a qualified job coach."),
  //                  new HtmlString("Free government scheme but you may have your own business costs"),
  //                  new HtmlString("Allows you to shape a job role that suits your business and the needs of the supported intern and increases your confidence of employing individuals with additional needs"),
  //                  new HtmlString("6 to 12-months"),
  //                  "supported-internships", 500),
  //              new Scheme("Care-Leaver covenant",
  //                  new HtmlString("Help 16 to 25-year-olds who were in local authority care become independent through practical job-related support, in whatever way suits your business."),
  //                  new HtmlString("Free government scheme but you may have your own business costs related to the practical offer you make to care leavers"),
  //                  new HtmlString("Helping care-leavers to move to independent living and bring their life skills to enrich your business"),
  //                  new HtmlString("Length of time can vary"),
  //                  "care-leaver-covenant", 400),
  //              new Scheme("Employing prisoners and prison leavers",
  //                  new HtmlString("Employ prisoners and prison leavers aged 18+ to help your business fill skills gaps and develop a loyal and talented workforce."),
  //                  new HtmlString("Free government scheme but you may have your own business costs"),
  //                  new HtmlString("Helping prisoners and prison leavers find employment"),
  //                  new HtmlString("Variable depending on employer and prisoner and prison leaver"),
  //                  "employing-prisoners-and-prison-leavers", 300),
  //              new Scheme("Training outside of employment",
  //                  new HtmlString("Additional ways to train up existing employees through free qualifications, career advice and financial support."),
  //                  new HtmlString("Courses are fully funded for eligible adults but employees may need time to study and attend classes"),
  //                  new HtmlString("Help improve productivity, and develop a loyal and talented workforce in a rapidly changing labour market"),
  //                  new HtmlString("Length of time can vary"),
  //                  "training-outside-of-employment", 200),
  //              new Scheme("Higher Technical Qualifications (HTQs)",
  //                  new HtmlString("Level 4 or 5 qualifications, for over 18s, with no work placement but flexible for employees to study while working."),
  //                  new HtmlString("Free government scheme but you may have your own business costs"),
  //                  new HtmlString("Developed by employers to upskill existing employees, provide a different recruitment pool to hire new talent and help your company succeed."),
  //                  new HtmlString("1 to 2 years"),
  //                  "higher-technical-qualifications", 100)
            };
    }
}
