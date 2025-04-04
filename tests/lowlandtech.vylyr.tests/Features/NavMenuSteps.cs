namespace LowlandTech.Vylyr.Tests.Features;

[Binding]
public class NavMenuSteps
{
    private readonly NavMenuVm _vm = new();
    private List<GraphNodeType> _nodeTypes = new();

    [Given("the application has loaded")]
    public void GivenTheApplicationHasLoaded()
    {
        _vm.Should().NotBeNull();
    }

    [Given("the navigation menu is visible")]
    public void GivenTheNavigationMenuIsVisible()
    {
        // Placeholder - UI concern
    }

    [Given("the FooterMode is None")]
    public void GivenTheFooterModeIsNone()
    {
        _vm.ResetFooter();
    }

    [When("the drawer is open")]
    public void WhenTheDrawerIsOpen()
    {
        // Placeholder - UI concern
    }

    [Then("the floating MenuPanelActions should be visible")]
    public void ThenTheFloatingMenuPanelActionsShouldBeVisible()
    {
        _vm.FooterMode.Should().Be(FooterMode.None);
    }

    [When("the user clicks the filter FAB")]
    public void WhenTheUserClicksTheFilterFAB()
    {
        _vm.SetFooterMode(FooterMode.Filter);
    }

    [Then("FooterMode should be set to Filter")]
    public void ThenFooterModeShouldBeSetToFilter()
    {
        _vm.FooterMode.Should().Be(FooterMode.Filter);
    }

    [Then("FilterNode should be displayed in the drawer footer")]
    public void ThenFilterNodeShouldBeDisplayed()
    {
        _vm.FooterMode.Should().Be(FooterMode.Filter);
    }

    [When("the user clicks the new node FAB")]
    public void WhenTheUserClicksTheNewNodeFAB()
    {
        _vm.SetFooterMode(FooterMode.NewNode);
    }

    [Then("the NewNode dialog should open")]
    public void ThenTheNewNodeDialogShouldOpen()
    {
        _vm.FooterMode.Should().Be(FooterMode.NewNode);
    }

    [Then("MenuPanelActions should not be visible")]
    public void ThenMenuPanelActionsShouldNotBeVisible()
    {
        _vm.FooterMode.Should().NotBe(FooterMode.None);
    }

    [When("the user clicks the close button in the footer")]
    public void WhenTheUserClicksTheCloseButton()
    {
        _vm.ResetFooter();
    }

    [Then("FooterMode should be set to None")]
    public void ThenFooterModeShouldBeNone()
    {
        _vm.FooterMode.Should().Be(FooterMode.None);
    }

    [Given("the user has opened the FilterNode")]
    public void GivenTheUserHasOpenedTheFilterNode()
    {
        _vm.SetFooterMode(FooterMode.Filter);
    }

    [When("the user navigates to a child node")]
    public void WhenTheUserNavigatesToAChildNode()
    {
        _vm.ResetFooter(); // Simulate MenuPanel behavior
    }

    [Given("the user has submitted a new node form")]
    public void GivenTheUserHasSubmittedANewNode()
    {
        _nodeTypes = new List<GraphNodeType>
        {
            new GraphNodeType { Id = "list", Label = "List" },
            new GraphNodeType { Id = "note", Label = "Note" }
        };

        _vm.ResetNewNode(_nodeTypes);
        _vm.NewNode.Title = "My new node";
        _vm.ResetNewNode(_nodeTypes); // Simulate post-submit
    }

    [Then("the GraphNode model should reset with an empty title")]
    public void ThenNewNodeShouldHaveEmptyTitle()
    {
        _vm.NewNode.Title.Should().BeEmpty();
    }

    [Then("the default node type \"(.*)\" should be selected")]
    public void ThenDefaultTypeShouldBe(string typeId)
    {
        _vm.NewNode.Type.Id.Should().Be(typeId);
    }

    [Given("the app provides a list of GraphNodeTypes")]
    public void GivenNodeTypesAreProvided()
    {
        _nodeTypes = new List<GraphNodeType>
        {
            new GraphNodeType { Id = "list", Label = "List" },
            new GraphNodeType { Id = "note", Label = "Note" }
        };
    }

    [When("the NewNode or FilterNode component is shown")]
    public void WhenNodeComponentIsShown()
    {
        _vm.ResetNewNode(_nodeTypes);
    }

    [Then("the type selector should be populated with those types")]
    public void ThenTypeSelectorShouldBePopulated()
    {
        _vm.AvailableTypes.Should().HaveCount(2);
    }
}