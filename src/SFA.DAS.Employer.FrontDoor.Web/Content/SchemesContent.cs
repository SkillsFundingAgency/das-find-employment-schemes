﻿using System.Collections.Generic;
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
                    new HtmlString("Paid employment for over 16s combining work and study in a specific job allowing you to develop your workforce and business."),
                    new HtmlString("Apprentice minimum wage and 5% training contribution depending on business size"),
                    new HtmlString("You develop a motivated, skilled and qualified workforce"),
                    new HtmlString("Minimum of 12 months employment"),
                    "apprenticeships", 1000, new HtmlString(""),
                    new HtmlString(@"<p class=""govuk-body"">
			Applies to England
		</p>
		<p class=""govuk-body"">
			<a href=""https://www.apprenticeships.gov.uk/employers"" class=""govuk-link"">Apprenticeships</a> are for those aged 16 or over and combine working with studying to gain skills and knowledge in a job role.
		</p>
		<p class=""govuk-body"">
			<a href=""https://www.gov.uk/apprenticeships-guide"" class=""govuk-link""> Apprentices</a> can be new or current employees. 
		</p>
		<p class=""govuk-body"">
			Your apprentice must: 
		</p>
		<ul class=""govuk-list govuk-list--bullet"">
			<li>work with experienced staff</li>
			<li>learn skills relevant to your organisation</li>
			<li>get time for training or study during their working week (at least 20% of their normal working hours)</li>
		</ul>"),
                new HtmlString(@"<p class=""govuk-body"">
            You must pay apprentices <a href=""https://www.gov.uk/national-minimum-wage-rates"" class=""govuk-link"">the minimum wage</a>.
		</p>
		<p class=""govuk-body"">
            You may have to contribute 5% of the apprentice’s training and assessment costs – depending on the size of your business and the age of your apprentice.
		</p>"),
                new HtmlString(@"<p class=""govuk-body"">
			Employers must provide:
		</p>
		<p class=""govuk-body"">
		</p>
		<ul class=""govuk-list govuk-list--bullet"">
			<li>a safe work environment</li>
			<li>an induction which includes explaining relevant policies and procedures</li>
			<li>opportunities for the student to develop their technical skills within your industry</li>
			<li>20% ‘Off-the-job’ training delivered by a training provider during your apprentice's normal working hours</li>
			<li>‘On-the-job’ training and additional support to ensure your apprentice’s success</li>
		</ul>
		<p class=""govuk-body"">
			Employers must also have:
		</p>
		<p class=""govuk-body"">
		</p>
		<ul class=""govuk-list govuk-list--bullet"">
			<li>a signed apprenticeship agreement and apprenticeship commitment statement from the learner and training provider</li>
		</ul>"),
                    new HtmlString(@"<p class=""govuk-body"">
			Find out <a href=""https://www.apprenticeships.gov.uk/"" class=""govuk-link"">how apprenticeships can benefit your business</a>.
		</p>"),
                    new HtmlString(@"<p class=""govuk-body"">
			Read about <a href=""https://www.gov.uk/government/case-studies/how-employers-are-making-the-most-of-apprenticeship-incentives"" class=""govuk-link"">how businesses are using incentive payments to hire apprentices</a>.
		</p>"),
                    "Offer an apprenticeship",
                    //todo: we won't be able to add "govuk-!-margin-bottom-0" on the last <p> from contentful
                    // so we'll have to replace das-highlight with a cx-cta-box or similar with different bottom padding
                    new HtmlString(@"<p class=""govuk-body"">
				Get started with offering an apprenticeship.
			</p>
			<p class=""govuk-body"">
				<a href=""https://www.apprenticeships.gov.uk/employers"" class=""govuk-link""> Hire an apprentice </a>
			</p>
            <p class=""govuk-body"">
				Or call <a href=""tel:08000150400"" class=""govuk-link"">08000 150 400</a>
            </p>")),
                new Scheme("T Levels: industry placements",
                    new HtmlString("Provide a 45 day (315 hours) industry placement for 16 to 19 year-olds, which gives you early access to the brightest entering the market and the opportunity to develop your workforce of the future."),
                    new HtmlString("Free government scheme but you may have your own business costs"),
                    new HtmlString("Industry placements can save on many of the costs associated with recruitment"),
                    new HtmlString("Short term 45-day industry placement"),
                    "t-levels", 900, new HtmlString(""),
                    new HtmlString(@"<p class=""govuk-body"">
				        <a href=""https://www.tlevels.gov.uk/employers"" class=""govuk-link"">T Levels</a> are qualifications for students aged 16 to 19 who have finished GCSEs. They are a 2-year qualification and the equivalent to 3 A levels.
                    </p>
                    <p class=""govuk-body"">
                        They have been designed with employers, to give them the skilled workforce they need by helping young people develop their knowledge, attitude and practical skills to thrive in the workplace.
                    </p>"),
                    new HtmlString(@"<p class=""govuk-body"">
                    Free government scheme but you may have your own business costs.
                    </p>
                    <p class=""govuk-body"">
                    Some employers may choose to pay wages and cover expenses such as travel and living costs.
                    </p>"),
                    new HtmlString(@""),
                    new HtmlString(@""),
                    new HtmlString(@""),
                    "",
                    new HtmlString(@"")),
                new Scheme("Traineeships",
                    new HtmlString("A 6-week to 12 months skills development programme that includes an unpaid work placement to help 16- to 24-year-olds prepare for an apprenticeship or employment."),
                    new HtmlString("Free government scheme but you may have your own business costs"),
                    new HtmlString("Helps you increase your capacity and productivity whilst developing a loyal and talented workforce"),
                    new HtmlString("6-week to 12 months"),
                    "traineeships", 800),
                new Scheme("Skills Bootcamps",
                    new HtmlString("Flexible work and training courses for over 19’s to fast-track specialist skill development, for existing or new talent for your business."),
                    new HtmlString("No cost unless self-employed"),
                    new HtmlString("Helps future-proof your business in a rapidly changing labour market and recruit staff with the right training and skills from the outset"),
                    new HtmlString("12 to 16-weeks"),
                    "skills-bootcamps", 700),
                    new Scheme("Sector-based Work Academy Programme (SWAP)",
                    new HtmlString("Up to 6-week placement for benefit claimants aged 18+ designed to help you recruit a workforce with the right skills to sustain and grow your business."),
                    new HtmlString("Free government scheme but you may have your own business costs"),
                    new HtmlString("Helps you recruit or train staff through fully-funded pre-employment training"),
                    new HtmlString("Up to 6-weeks"),
                    "swap", 600),
                new Scheme("Supported Internships for learners with an education, health and care plan",
                    new HtmlString("An unpaid 6 to 12-month work placement for 16 to 24-year-olds with additional needs supported by a qualified job coach."),
                    new HtmlString("Free government scheme but you may have your own business costs"),
                    new HtmlString("Allows you to shape a job role that suits your business and the needs of the supported intern and increases your confidence of employing individuals with additional needs"),
                    new HtmlString("6 to 12-months"),
                    "supported-internships", 500),
                new Scheme("Care-Leaver covenant",
                    new HtmlString("Help 16 to 25-year-olds who were in local authority care become independent through practical job-related support, in whatever way suits your business."),
                    new HtmlString("Free government scheme but you may have your own business costs related to the practical offer you make to care leavers"),
                    new HtmlString("Helping care-leavers to move to independent living and bring their life skills to enrich your business"),
                    new HtmlString("Length of time can vary"),
                    "care-leaver-covenant", 400),
                new Scheme("Employing prisoners and prison leavers",
                    new HtmlString("Employ prisoners and prison leavers aged 18+ to help your business fill skills gaps and develop a loyal and talented workforce."),
                    new HtmlString("Free government scheme but you may have your own business costs"),
                    new HtmlString("Helping prisoners and prison leavers find employment"),
                    new HtmlString("Variable depending on employer and prisoner and prison leaver"),
                    "employing-prisoners-and-prison-leavers", 300),
                new Scheme("Training outside of employment",
                    new HtmlString("Additional ways to train up existing employees through free qualifications, career advice and financial support."),
                    new HtmlString("Courses are fully funded for eligible adults but employees may need time to study and attend classes"),
                    new HtmlString("Help improve productivity, and develop a loyal and talented workforce in a rapidly changing labour market"),
                    new HtmlString("Length of time can vary"),
                    "training-outside-of-employment", 200),
                new Scheme("Higher Technical Qualifications (HTQs)",
                    new HtmlString("Level 4 or 5 qualifications, for over 18s, with no work placement but flexible for employees to study while working."),
                    new HtmlString("Free government scheme but you may have your own business costs"),
                    new HtmlString("Developed by employers to upskill existing employees, provide a different recruitment pool to hire new talent and help your company succeed."),
                    new HtmlString("1 to 2 years"),
                    "higher-technical-qualifications", 100)
            };
    }
}
