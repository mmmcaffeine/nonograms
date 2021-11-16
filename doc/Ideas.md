# Strategies

## `TrivialStrategy`

* This should still work if some cells are excluded and the hint fits the remaining cells

## `GlueStrategy`

* This could be expanded to cope with multi-part hints e.g. "2,2" could be used to solve "1___1"
* If there is only one element in hint you can eliminate all other cells
* If there are only two elements in hint you can eliminate all other cells if both ends are glued
* If there is only one element in hint but cells are glued at both extremes you should do nothing
* This is probably not too efficient in terms of how it is managing memory and could almost certainly be improved

# Representing Data

* You could represent a hint as "2,2"
* You could represent the cells as a string e.g. "100_1" to represent one filled cell, two excluded cells, one unknown cell, and finally another filled cell
* Representing the hint and the cells as strings might make it easier to set up tests because we could then use `InlineData`
    * This would keep the test data closer to the test
    * This would keep test data shorter so having more complex test cases is more practical

# General Things

* Would it make sense to try to pass data by reference? Should there be something like a `Solver` that has the responsibility for changing values in stored data based on what the strategy says
* There is going to be common validation in strategies so an abstract `StrategyBase` class might make sense
    * Testing the `hint` and `cells` parameters for nulls will be duplicated
    * Testing the `hint` and `cells` parameters for at least one item will be duplicated
    * Testing the `hint` wouldn't require more cells than exist will be duplicated
    * Testing the solution for the row doesn't conflict with what is already there will be duplicated
    * Parsing strings into `int[]` and `bool?[]` would be duplicated (should you choose to put that on the interface)
* Do I want to change the design of the interface so I don't have to keep turning `IEnumerable<T>` into `t[]`?
    * `ICollection<T>` has a `Count` property which is really what I'm after most of the time
    * Could I have an efficient method in any `StrategyBase` type that would look at the underlying type of the `IEnumerable<T>` and cast to `ICollection<T>` or `Array<T>` if possible, and convert if not?
* Using `bool?` to represent cells is creating some friction. I cannot just create an array of nulls as the compiler fails to infer a type. I have to manually specify the type of the parameter which is a nuisance