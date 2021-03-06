using System;
using System.ComponentModel;
using StoryQ.Infrastructure;
using StoryQEntryPoints = Spinit.Wpc.Synologen.Presentation.Intranet.AcceptanceTest.TestHelpers.Infrastructure.StoryQEntryPoints;

// tells the parser what our entry points are:
[assembly:ParserEntryPointAttribute(typeof(StoryQEntryPoints))]

namespace Spinit.Wpc.Synologen.Presentation.Intranet.AcceptanceTest.TestHelpers
{

    /// <summary>
    /// The [Berättelse] story fragment.
    /// This is the root item of any story
    /// <h1>Transitions:</h1><ul>
    /// <li>for att [<see cref="Nytta"/>]: <see cref="FörAtt(string)"/></li>
    /// </ul>
    /// </summary>
    public class Berättelse : FragmentBase
    {
        /// <summary>
        /// Starts a new StoryQ Berättelse. 
        /// </summary>
        /// <param name="text">The name of the new Berättelse</param>

        public Berättelse(string text):base(new Step("Berättelse", 0, text, Step.DoNothing), null){}

        /// <summary>
        /// för att [Nytta].
        /// Describe the real-world value for this story. What is the business process that the user requires software support from?
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Nytta"/></returns>
        [Description("Describe the real-world value for this story. What is the business process that the user requires software support from?")]
        public Nytta FörAtt(string text)
        {
            Step s = new Step("För att", 1, text, Step.DoNothing);
            return new Nytta(s, this);
        }

        /// <summary>
        /// Adds a tag to this step. Tags can be used make disparate steps searchable.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Berättelse Tag(string tag)
        {
            ((IStepContainer)this).Step.Tags.Add(tag.Trim().Trim('#'));
            return this;
        }

    }

    /// <summary>
    /// The [Nytta] story fragment.
    /// The real-world objective (business value) of a story
    /// <h1>Transitions:</h1><ul>
    /// <li>och [<see cref="Nytta"/>]: <see cref="Och(string)"/></li>
    /// <li>som [<see cref="Roll"/>]: <see cref="Som(string)"/></li>
    /// </ul>
    /// </summary>
    public class Nytta : FragmentBase
    {
        internal Nytta(Step step, IStepContainer parent):base(step, parent){}

        /// <summary>
        /// och [Nytta].
        /// Describe any secondary business functions that this story will support
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Nytta"/></returns>
        [Description("Describe any secondary business functions that this story will support")]
        public Nytta Och(string text)
        {
            Step s = new Step("Och", 2, text, Step.DoNothing);
            return new Nytta(s, this);
        }


        /// <summary>
        /// som [Roll].
        /// The role of the person who is the intended user of this feature
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Roll"/></returns>
        [Description("The role of the person who is the intended user of this feature")]
        public Roll Som(string text)
        {
            Step s = new Step("Som", 1, text, Step.DoNothing);
            return new Roll(s, this);
        }

        /// <summary>
        /// Adds a tag to this step. Tags can be used make disparate steps searchable.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Nytta Tag(string tag)
        {
            ((IStepContainer)this).Step.Tags.Add(tag.Trim().Trim('#'));
            return this;
        }

    }

    /// <summary>
    /// The [Roll] story fragment.
    /// The role (a category of actors/users) or roles that receive this benefit. 
    /// <h1>Transitions:</h1><ul>
    /// <li>eller som [<see cref="Roll"/>]: <see cref="EllerSom(string)"/></li>
    /// <li>vill jag [<see cref="Funktion"/>]: <see cref="VillJag(string)"/></li>
    /// </ul>
    /// </summary>
    public class Roll : FragmentBase
    {
        internal Roll(Step step, IStepContainer parent):base(step, parent){}

        /// <summary>
        /// eller som [Roll].
        /// Any other roles that may use this story
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Roll"/></returns>
        [Description("Any other roles that may use this story")]
        public Roll EllerSom(string text)
        {
            Step s = new Step("Eller som", 2, text, Step.DoNothing);
            return new Roll(s, this);
        }


        /// <summary>
        /// vill jag [Funktion].
        /// Describe the software process (features) that will support the business requirement
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Funktion"/></returns>
        [Description("Describe the software process (features) that will support the business requirement")]
        public Funktion VillJag(string text)
        {
            Step s = new Step("Vill jag", 1, text, Step.DoNothing);
            return new Funktion(s, this);
        }

        /// <summary>
        /// Adds a tag to this step. Tags can be used make disparate steps searchable.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Roll Tag(string tag)
        {
            ((IStepContainer)this).Step.Tags.Add(tag.Trim().Trim('#'));
            return this;
        }

    }

    /// <summary>
    /// The [Funktion] story fragment.
    /// The software process that will implement the specified benefit.
    /// <h1>Transitions:</h1><ul>
    /// <li>och [<see cref="Funktion"/>]: <see cref="Och(string)"/></li>
    /// <li>med scenario [<see cref="Scenario"/>]: <see cref="MedScenario(string)"/></li>
    /// </ul>
    /// </summary>
    public class Funktion : FragmentBase
    {
        internal Funktion(Step step, IStepContainer parent):base(step, parent){}

        /// <summary>
        /// och [Funktion].
        /// Any other features that will implement the desired benefit
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Funktion"/></returns>
        [Description("Any other features that will implement the desired benefit")]
        public Funktion Och(string text)
        {
            Step s = new Step("Och", 2, text, Step.DoNothing);
            return new Funktion(s, this);
        }


        /// <summary>
        /// med scenario [Scenario].
        /// Add a scenario ('given'/'when'/'then') to this story. Scenarios can be added (and will be run) in sequence. Each scenario should have a short descriptive name.
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Scenario"/></returns>
        [Description("Add a scenario ('given'/'when'/'then') to this story. Scenarios can be added (and will be run) in sequence. Each scenario should have a short descriptive name.")]
        public Scenario MedScenario(string text)
        {
            Step s = new Step("Med scenario", 3, text, Step.DoNothing);
            return new Scenario(s, this);
        }

        /// <summary>
        /// Adds a tag to this step. Tags can be used make disparate steps searchable.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Funktion Tag(string tag)
        {
            ((IStepContainer)this).Step.Tags.Add(tag.Trim().Trim('#'));
            return this;
        }

    }

    /// <summary>
    /// The [Scenario] story fragment.
    /// The name of each scenario within a story. You can think of each scenario as a chapter in a book.
    /// <h1>Transitions:</h1><ul>
    /// <li>givet [<see cref="Forutsattning"/>]: <see cref="Givet(Action)"/></li>
    /// </ul>
    /// </summary>
    public class Scenario : FragmentBase
    {
        internal Scenario(Step step, IStepContainer parent):base(step, parent){}

        /// <summary>
        /// Givet [Förutsattning].
        /// Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state")]
        public Förutsattning Givet(Action descriptiveAction)
        {
            string text = MethodToText(descriptiveAction);
            Step s = new Step("Givet", 4, text, descriptiveAction);
            return new Förutsattning(s, this);
        }

        /// <summary>
        /// Givet [Förutsattning].
        /// Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state")]
        public Förutsattning Givet<T1>(Action<T1> descriptiveAction, T1 arg1)
        {
            string text = MethodToText(descriptiveAction, arg1);
            Step s = new Step("Givet", 4, text, () => descriptiveAction(arg1));
            return new Förutsattning(s, this);
        }

        /// <summary>
        /// Givet [Förutsattning].
        /// Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state")]
        public Förutsattning Givet<T1, T2>(Action<T1, T2> descriptiveAction, T1 arg1, T2 arg2)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2);
            Step s = new Step("Givet", 4, text, () => descriptiveAction(arg1, arg2));
            return new Förutsattning(s, this);
        }

        /// <summary>
        /// Givet [Förutsattning].
        /// Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state")]
        public Förutsattning Givet<T1, T2, T3>(Action<T1, T2, T3> descriptiveAction, T1 arg1, T2 arg2, T3 arg3)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3);
            Step s = new Step("Givet", 4, text, () => descriptiveAction(arg1, arg2, arg3));
            return new Förutsattning(s, this);
        }

        /// <summary>
        /// Givet [Förutsattning].
        /// Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg4">The fourth argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state")]
        public Förutsattning Givet<T1, T2, T3, T4>(Action<T1, T2, T3, T4> descriptiveAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3, arg4);
            Step s = new Step("Givet", 4, text, () => descriptiveAction(arg1, arg2, arg3, arg4));
            return new Förutsattning(s, this);
        }

        /// <summary>
        /// Givet [Förutsattning].
        /// Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state")]
        protected Förutsattning Givet(string text)
        {
            Step s = new Step("Givet", 4, text, null);
            return new Förutsattning(s, this);
        }

        /// <summary>
        /// Adds a tag to this step. Tags can be used make disparate steps searchable.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Scenario Tag(string tag)
        {
            ((IStepContainer)this).Step.Tags.Add(tag.Trim().Trim('#'));
            return this;
        }

    }

    /// <summary>
    /// The [Forutsattning] story fragment.
    /// The preconditions that are meant to be present at the beginning of the scenario.
    /// <h1>Transitions:</h1><ul>
    /// <li>och [<see cref="Forutsattning"/>]: <see cref="Och(Action)"/></li>
    /// <li>nar [<see cref="Operation"/>]: <see cref="Nar(Action)"/></li>
    /// </ul>
    /// </summary>
    public class Förutsattning : FragmentBase
    {
        internal Förutsattning(Step step, IStepContainer parent):base(step, parent){}

        /// <summary>
        /// Och [Forutsattning].
        /// Provide another precondition to describe our scenario's initial state.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide another precondition to describe our scenario's initial state")]
        public Förutsattning Och(Action descriptiveAction)
        {
            string text = MethodToText(descriptiveAction);
            Step s = new Step("Och", 5, text, descriptiveAction);
            return new Förutsattning(s, this);
        }

        /// <summary>
        /// Och [Förutsattning].
        /// Provide another precondition to describe our scenario's initial state.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide another precondition to describe our scenario's initial state")]
        public Förutsattning Och<T1>(Action<T1> descriptiveAction, T1 arg1)
        {
            string text = MethodToText(descriptiveAction, arg1);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1));
            return new Förutsattning(s, this);
        }

        /// <summary>
        /// och [Förutsattning].
        /// Provide another precondition to describe our scenario's initial state.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide another precondition to describe our scenario's initial state")]
        public Förutsattning Och<T1, T2>(Action<T1, T2> descriptiveAction, T1 arg1, T2 arg2)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1, arg2));
            return new Förutsattning(s, this);
        }

        /// <summary>
        /// Och [Förutsattning].
        /// Provide another precondition to describe our scenario's initial state.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide another precondition to describe our scenario's initial state")]
        public Förutsattning Och<T1, T2, T3>(Action<T1, T2, T3> descriptiveAction, T1 arg1, T2 arg2, T3 arg3)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1, arg2, arg3));
            return new Förutsattning(s, this);
        }

        /// <summary>
        /// Och [Förutsattning].
        /// Provide another precondition to describe our scenario's initial state.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg4">The fourth argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide another precondition to describe our scenario's initial state")]
        public Förutsattning Och<T1, T2, T3, T4>(Action<T1, T2, T3, T4> descriptiveAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3, arg4);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1, arg2, arg3, arg4));
            return new Förutsattning(s, this);
        }

        /// <summary>
        /// Och [Förutsattning].
        /// Provide another precondition to describe our scenario's initial state
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
        [Description("Provide another precondition to describe our scenario's initial state")]
        protected Förutsattning Och(string text)
        {
            Step s = new Step("Och", 5, text, null);
            return new Förutsattning(s, this);
        }


        /// <summary>
        /// När [Operation].
        /// Describe the actions that are done <em>to</em> the system under test. '.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Describe the actions that are done <em>to</em> the system under test. '")]
        public Operation När(Action descriptiveAction)
        {
            string text = MethodToText(descriptiveAction);
            Step s = new Step("När", 4, text, descriptiveAction);
            return new Operation(s, this);
        }

        /// <summary>
        /// När [Operation].
        /// Describe the actions that are done <em>to</em> the system under test. '.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Describe the actions that are done <em>to</em> the system under test. '")]
        public Operation När<T1>(Action<T1> descriptiveAction, T1 arg1)
        {
            string text = MethodToText(descriptiveAction, arg1);
            Step s = new Step("När", 4, text, () => descriptiveAction(arg1));
            return new Operation(s, this);
        }

        /// <summary>
        /// När [Operation].
        /// Describe the actions that are done <em>to</em> the system under test. '.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Describe the actions that are done <em>to</em> the system under test. '")]
        public Operation När<T1, T2>(Action<T1, T2> descriptiveAction, T1 arg1, T2 arg2)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2);
            Step s = new Step("När", 4, text, () => descriptiveAction(arg1, arg2));
            return new Operation(s, this);
        }

        /// <summary>
        /// När [Operation].
        /// Describe the actions that are done <em>to</em> the system under test. '.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Describe the actions that are done <em>to</em> the system under test. '")]
        public Operation När<T1, T2, T3>(Action<T1, T2, T3> descriptiveAction, T1 arg1, T2 arg2, T3 arg3)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3);
            Step s = new Step("När", 4, text, () => descriptiveAction(arg1, arg2, arg3));
            return new Operation(s, this);
        }

        /// <summary>
        /// När [Operation].
        /// Describe the actions that are done <em>to</em> the system under test. '.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg4">The fourth argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Describe the actions that are done <em>to</em> the system under test. '")]
        public Operation När<T1, T2, T3, T4>(Action<T1, T2, T3, T4> descriptiveAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3, arg4);
            Step s = new Step("När", 4, text, () => descriptiveAction(arg1, arg2, arg3, arg4));
            return new Operation(s, this);
        }

        /// <summary>
        /// När [Operation].
        /// Describe the actions that are done <em>to</em> the system under test. '
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Describe the actions that are done <em>to</em> the system under test. '")]
        protected Operation När(string text)
        {
            Step s = new Step("När", 4, text, null);
            return new Operation(s, this);
        }

        /// <summary>
        /// Adds a tag to this step. Tags can be used make disparate steps searchable.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Förutsattning Tag(string tag)
        {
            ((IStepContainer)this).Step.Tags.Add(tag.Trim().Trim('#'));
            return this;
        }

    }

    /// <summary>
    /// The [Operation] story fragment.
    /// The action(s) that are performed upon the system under test
    /// <h1>Transitions:</h1><ul>
    /// <li>och [<see cref="Operation"/>]: <see cref="Och(Action)"/></li>
    /// <li>sa [<see cref="Utfall"/>]: <see cref="Sa(Action)"/></li>
    /// </ul>
    /// </summary>
    public class Operation : FragmentBase
    {
        internal Operation(Step step, IStepContainer parent):base(step, parent){}

        /// <summary>
        /// Och [Operation].
        /// Provide another action that is to be performed on the system, prior to our check for behaviour ('then').
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Provide another action that is to be performed on the system, prior to our check for behaviour ('then')")]
        public Operation Och(Action descriptiveAction)
        {
            string text = MethodToText(descriptiveAction);
            Step s = new Step("Och", 5, text, descriptiveAction);
            return new Operation(s, this);
        }

        /// <summary>
        /// Och [Operation].
        /// Provide another action that is to be performed on the system, prior to our check for behaviour ('then').
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Provide another action that is to be performed on the system, prior to our check for behaviour ('then')")]
        public Operation Och<T1>(Action<T1> descriptiveAction, T1 arg1)
        {
            string text = MethodToText(descriptiveAction, arg1);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1));
            return new Operation(s, this);
        }

        /// <summary>
        /// Och [Operation].
        /// Provide another action that is to be performed on the system, prior to our check for behaviour ('then').
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Provide another action that is to be performed on the system, prior to our check for behaviour ('then')")]
        public Operation Och<T1, T2>(Action<T1, T2> descriptiveAction, T1 arg1, T2 arg2)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1, arg2));
            return new Operation(s, this);
        }

        /// <summary>
        /// Och [Operation].
        /// Provide another action that is to be performed on the system, prior to our check for behaviour ('then').
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Provide another action that is to be performed on the system, prior to our check for behaviour ('then')")]
        public Operation Och<T1, T2, T3>(Action<T1, T2, T3> descriptiveAction, T1 arg1, T2 arg2, T3 arg3)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1, arg2, arg3));
            return new Operation(s, this);
        }

        /// <summary>
        /// Och [Operation].
        /// Provide another action that is to be performed on the system, prior to our check for behaviour ('then').
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg4">The fourth argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Provide another action that is to be performed on the system, prior to our check for behaviour ('then')")]
        public Operation Och<T1, T2, T3, T4>(Action<T1, T2, T3, T4> descriptiveAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3, arg4);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1, arg2, arg3, arg4));
            return new Operation(s, this);
        }

        /// <summary>
        /// Och [Operation].
        /// Provide another action that is to be performed on the system, prior to our check for behaviour ('then')
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
        [Description("Provide another action that is to be performed on the system, prior to our check for behaviour ('then')")]
        protected Operation Och(string text)
        {
            Step s = new Step("Och", 5, text, null);
            return new Operation(s, this);
        }


        /// <summary>
        /// Så [Utfall].
        /// Describe the system's behaviour that the prior state and actions should elicit.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Describe the system's behaviour that the prior state and actions should elicit")]
        public Utfall Så(Action descriptiveAction)
        {
            string text = MethodToText(descriptiveAction);
            Step s = new Step("Så", 4, text, descriptiveAction);
            return new Utfall(s, this);
        }

        /// <summary>
        /// Så [Utfall].
        /// Describe the system's behaviour that the prior state and actions should elicit.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Describe the system's behaviour that the prior state and actions should elicit")]
        public Utfall Så<T1>(Action<T1> descriptiveAction, T1 arg1)
        {
            string text = MethodToText(descriptiveAction, arg1);
            Step s = new Step("Så", 4, text, () => descriptiveAction(arg1));
            return new Utfall(s, this);
        }

        /// <summary>
        /// Så [Utfall].
        /// Describe the system's behaviour that the prior state and actions should elicit.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Describe the system's behaviour that the prior state and actions should elicit")]
        public Utfall Så<T1, T2>(Action<T1, T2> descriptiveAction, T1 arg1, T2 arg2)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2);
            Step s = new Step("Så", 4, text, () => descriptiveAction(arg1, arg2));
            return new Utfall(s, this);
        }

        /// <summary>
        /// Så [Utfall].
        /// Describe the system's behaviour that the prior state and actions should elicit.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Describe the system's behaviour that the prior state and actions should elicit")]
        public Utfall Så<T1, T2, T3>(Action<T1, T2, T3> descriptiveAction, T1 arg1, T2 arg2, T3 arg3)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3);
            Step s = new Step("Så", 4, text, () => descriptiveAction(arg1, arg2, arg3));
            return new Utfall(s, this);
        }

        /// <summary>
        /// Så [Utfall].
        /// Describe the system's behaviour that the prior state and actions should elicit.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg4">The fourth argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Describe the system's behaviour that the prior state and actions should elicit")]
        public Utfall Så<T1, T2, T3, T4>(Action<T1, T2, T3, T4> descriptiveAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3, arg4);
            Step s = new Step("Så", 4, text, () => descriptiveAction(arg1, arg2, arg3, arg4));
            return new Utfall(s, this);
        }

        /// <summary>
        /// Så [Utfall].
        /// Describe the system's behaviour that the prior state and actions should elicit
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Describe the system's behaviour that the prior state and actions should elicit")]
        protected Utfall Så(string text)
        {
            Step s = new Step("Så", 4, text, null);
            return new Utfall(s, this);
        }

        /// <summary>
        /// Adds a tag to this step. Tags can be used make disparate steps searchable.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Operation Tag(string tag)
        {
            ((IStepContainer)this).Step.Tags.Add(tag.Trim().Trim('#'));
            return this;
        }

    }

    /// <summary>
    /// The [Utfall] story fragment.
    /// The result that is expected from executing the specified actions on the specified initial state
    /// <h1>Transitions:</h1><ul>
    /// <li>och [<see cref="Utfall"/>]: <see cref="Och(Action)"/></li>
    /// <li>med scenario [<see cref="Scenario"/>]: <see cref="MedScenario(string)"/></li>
    /// </ul>
    /// </summary>
    public class Utfall : FragmentBase
    {
        internal Utfall(Step step, IStepContainer parent):base(step, parent){}

        /// <summary>
        /// Och [Utfall].
        /// Provide another resultant behaviour to check for.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Provide another resultant behaviour to check for")]
        public Utfall Och(Action descriptiveAction)
        {
            string text = MethodToText(descriptiveAction);
            Step s = new Step("Och", 5, text, descriptiveAction);
            return new Utfall(s, this);
        }

        /// <summary>
        /// Och [Utfall].
        /// Provide another resultant behaviour to check for.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Provide another resultant behaviour to check for")]
        public Utfall Och<T1>(Action<T1> descriptiveAction, T1 arg1)
        {
            string text = MethodToText(descriptiveAction, arg1);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1));
            return new Utfall(s, this);
        }

        /// <summary>
        /// Och [Utfall].
        /// Provide another resultant behaviour to check for.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Provide another resultant behaviour to check for")]
        public Utfall Och<T1, T2>(Action<T1, T2> descriptiveAction, T1 arg1, T2 arg2)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1, arg2));
            return new Utfall(s, this);
        }

        /// <summary>
        /// Och [Utfall].
        /// Provide another resultant behaviour to check for.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Provide another resultant behaviour to check for")]
        public Utfall Och<T1, T2, T3>(Action<T1, T2, T3> descriptiveAction, T1 arg1, T2 arg2, T3 arg3)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1, arg2, arg3));
            return new Utfall(s, this);
        }

        /// <summary>
        /// Och [Utfall].
        /// Provide another resultant behaviour to check for.
        /// </summary>
        /// <remarks>This overload infers its text from the name of the parameter <paramref name="descriptiveAction"/></remarks>
        /// <param name="descriptiveAction">
        /// A descriptively named method that should be run to fulfil this story fragment. The method's name will be used as the description for this fragment, once converted from CamelCase
        /// Any underscores in the method's name will be used as placeholders and will be replaced with the <see cref="object.ToString"/> of each respective argument.
        /// Do not use a lambda or anonymous method here, as the name will not be human readable
        /// </param>
        /// <param name="arg1">The first argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg2">The second argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg3">The third argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <param name="arg4">The fourth argument to be passed to <paramref name="descriptiveAction"/></param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Provide another resultant behaviour to check for")]
        public Utfall Och<T1, T2, T3, T4>(Action<T1, T2, T3, T4> descriptiveAction, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            string text = MethodToText(descriptiveAction, arg1, arg2, arg3, arg4);
            Step s = new Step("Och", 5, text, () => descriptiveAction(arg1, arg2, arg3, arg4));
            return new Utfall(s, this);
        }

        /// <summary>
        /// Och [Utfall].
        /// Provide another resultant behaviour to check for
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
        [Description("Provide another resultant behaviour to check for")]
        protected Utfall Och(string text)
        {
            Step s = new Step("Och", 5, text, null);
            return new Utfall(s, this);
        }


        /// <summary>
        /// Med scenario [Scenario].
        /// Add another scenario to this story. StoryQ executes these scenarios one after the other, so state <em>can</em> be shared between a single story's scenarios.
        /// </summary>
        /// <param name="text">
        /// A textual description. This story fragment is not executable.
        /// </param>
        /// <returns>The next fragment of your story, a <see cref="Scenario"/></returns>
        [Description("Add another scenario to this story. StoryQ executes these scenarios one after the other, so state <em>can</em> be shared between a single story's scenarios.")]
        public Scenario MedScenario(string text)
        {
            Step s = new Step("Med scenario", 3, text, Step.DoNothing);
            return new Scenario(s, this);
        }

        /// <summary>
        /// Adds a tag to this step. Tags can be used make disparate steps searchable.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Utfall Tag(string tag)
        {
            ((IStepContainer)this).Step.Tags.Add(tag.Trim().Trim('#'));
            return this;
        }

    }

    namespace TextualSteps
    {
        ///<summary>
        /// Extension methods to enable string-based executable steps. These will always Pend
        ///</summary>
        public static class Extensions
        {
            /// <summary>
            /// Givet [Forutsattning].
            /// Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state
            /// This story fragment should be executable, so a method is the preferred argument, but you can supply a string in the meantime. The step will Pend.
            /// </summary>
            /// <param name="parent">this</param>
            /// <param name="text">
            /// A textual description of the step. 
            /// </param>
            /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
            [Description("Provide the initial context to the scenario. Try not to describe behaviour or actions, this step describes and sets up initial state")]        
            public static Förutsattning Givet(this Scenario parent, string text)
            {				
                Step s = new Step("Givet", 4, text, () => { throw new StringBasedExecutableStepException(text); });
                return new Förutsattning(s, parent);
            }

            /// <summary>
            /// Och [Forutsattning].
            /// Provide another precondition to describe our scenario's initial state
            /// This story fragment should be executable, so a method is the preferred argument, but you can supply a string in the meantime. The step will Pend.
            /// </summary>
            /// <param name="parent">this</param>
            /// <param name="text">
            /// A textual description of the step. 
            /// </param>
            /// <returns>The next fragment of your story, a <see cref="Forutsattning"/></returns>
            [Description("Provide another precondition to describe our scenario's initial state")]        
            public static Förutsattning Och(this Förutsattning parent, string text)
            {				
                Step s = new Step("Och", 5, text, () => { throw new StringBasedExecutableStepException(text); });
                return new Förutsattning(s, parent);
            }

            /// <summary>
            /// När [Operation].
            /// Describe the actions that are done <em>to</em> the system under test. '
            /// This story fragment should be executable, so a method is the preferred argument, but you can supply a string in the meantime. The step will Pend.
            /// </summary>
            /// <param name="parent">this</param>
            /// <param name="text">
            /// A textual description of the step. 
            /// </param>
            /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
            [Description("Describe the actions that are done <em>to</em> the system under test. '")]        
            public static Operation När(this Förutsattning parent, string text)
            {				
                Step s = new Step("När", 4, text, () => { throw new StringBasedExecutableStepException(text); });
                return new Operation(s, parent);
            }

            /// <summary>
            /// Och [Operation].
            /// Provide another action that is to be performed on the system, prior to our check for behaviour ('then')
            /// This story fragment should be executable, so a method is the preferred argument, but you can supply a string in the meantime. The step will Pend.
            /// </summary>
            /// <param name="parent">this</param>
            /// <param name="text">
            /// A textual description of the step. 
            /// </param>
            /// <returns>The next fragment of your story, a <see cref="Operation"/></returns>
            [Description("Provide another action that is to be performed on the system, prior to our check for behaviour ('then')")]        
            public static Operation Och(this Operation parent, string text)
            {				
                Step s = new Step("Och", 5, text, () => { throw new StringBasedExecutableStepException(text); });
                return new Operation(s, parent);
            }

            /// <summary>
            /// Så [Utfall].
            /// Describe the system's behaviour that the prior state and actions should elicit
            /// This story fragment should be executable, so a method is the preferred argument, but you can supply a string in the meantime. The step will Pend.
            /// </summary>
            /// <param name="parent">this</param>
            /// <param name="text">
            /// A textual description of the step. 
            /// </param>
            /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
            [Description("Describe the system's behaviour that the prior state and actions should elicit")]        
            public static Utfall Så(this Operation parent, string text)
            {				
                Step s = new Step("Så", 4, text, () => { throw new StringBasedExecutableStepException(text); });
                return new Utfall(s, parent);
            }

            /// <summary>
            /// Och [Utfall].
            /// Provide another resultant behaviour to check for
            /// This story fragment should be executable, so a method is the preferred argument, but you can supply a string in the meantime. The step will Pend.
            /// </summary>
            /// <param name="parent">this</param>
            /// <param name="text">
            /// A textual description of the step. 
            /// </param>
            /// <returns>The next fragment of your story, a <see cref="Utfall"/></returns>
            [Description("Provide another resultant behaviour to check for")]        
            public static Utfall Och(this Utfall parent, string text)
            {				
                Step s = new Step("Och", 5, text, () => { throw new StringBasedExecutableStepException(text); });
                return new Utfall(s, parent);
            }

        }
    }

    namespace Infrastructure
    {
        /// <summary>
        /// Entry points for the StoryQ converter's parser
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class StoryQEntryPoints
        {
            /// <summary>
            /// For infrastructure use only
            /// </summary>
            [Description("This is the root item of any story")]
            protected Berättelse Berättelse(string text)
            {
                return new Berättelse(text);
            }

        }
    }
}

