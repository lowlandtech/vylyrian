Feature: Navigation Menu Behavior
  As a user of the Vylyr app
  I want the navigation menu to support floating actions and footer states
  So that I can filter and create nodes efficiently across devices

  Background:
    Given the application has loaded
    And the navigation menu is visible

  Scenario: Display MenuPanelActions when no footer is active
    Given the FooterMode is None
    When the drawer is open
    Then the floating MenuPanelActions should be visible

  Scenario: Show FilterNode in footer when filter FAB is clicked
    Given the FooterMode is None
    When the user clicks the filter FAB
    Then FooterMode should be set to Filter
    And FilterNode should be displayed in the drawer footer
    And MenuPanelActions should not be visible

  Scenario: Show NewNode as dialog when new node FAB is clicked
    Given the FooterMode is None
    When the user clicks the new node FAB
    Then the NewNode dialog should open
    And MenuPanelActions should not be visible

  Scenario: Hide footer and show FAB again when close is pressed
    Given the FilterNode is visible
    When the user clicks the close button in the footer
    Then FooterMode should be set to None
    And MenuPanelActions should be visible again

  Scenario: FAB and footer state persist across navigation
    Given the user has opened the FilterNode
    When the user navigates to a child node
    Then the FooterMode should reset to None
    And MenuPanelActions should be visible

  Scenario: NewNode model resets properly after creation
    Given the user has submitted a new node form
    Then the GraphNode model should reset with an empty title
    And the default node type "list" should be selected

  Scenario: Available node types are injected and selectable
    Given the app provides a list of GraphNodeTypes
    When the NewNode or FilterNode component is shown
    Then the type selector should be populated with those types
    And the default type should be "list"

