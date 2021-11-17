# Strategies

## `TrivialStrategy`

* This should still work if some cells are excluded and the hint fits the remaining cells

## `GlueStrategy`

* Would it be better to implement "right glue" by reversing the array, performing "left glue" then reversing again?
    * This would almost certainly be slower, but you wouldn't then have what is essentially a repeated algorithm
    * This idea would probably apply to other strategies too such as when you have e.g. "1,2" in a 5x5 grid and can say the cell at 4 is filled
* This could be expanded to cope with multi-part hints e.g. "2,2" could be used to solve "1___1"
* If there is only one element in hint you can eliminate all other cells
* If there are only two elements in hint you can eliminate all other cells if both ends are glued
* If there is only one element in hint but cells are glued at both extremes you should do nothing or throw an `InconsistentCellStateException`
* This is probably not too efficient in terms of how it is managing memory and could almost certainly be improved

## `NoSmallGapsStrategy`

* This could be improved by looking at the order of the hints
    * e.g. "5,1" and "0___0_____0_" should be able to eliminate the first gap because you know the 1 has to be to the right of the 5

## Testing

* Do I want to break up my tests and the assertion only checks part of the row?
    * e.g. for `GlueStrategy` differentiate checking the "glued" cells are filled and checking the cell to the immediate left or right of the glue is eliminated
* You need to test the input cells are not modified by the strategy

# Representing Data

* I've used `int`s for hints when negative or zero doesn't make sense; using a uint would probably make more sense
    * This probably doesn't even matter if I have a `Hint` type
* You could represent a hint as "2,2"
* You could represent the cells as a string e.g. "100_1" to represent one filled cell, two excluded cells, one unknown cell, and finally another filled cell
* Representing the hint and the cells as strings might make it easier to set up tests because we could then use `InlineData`
    * This would keep the test data closer to the test
    * This would keep test data shorter so having more complex test cases is more practical
* If I had a type of `Hint`
    * It would probably be a `record struct`
    * This would be a good place to have
        * Conversions between e.g. a string of "2,2" and an `int[]`
        * A constructor accepting a paramarry of `int`
        * The calculation for the minimum number of cells
    * This would make validation in `StrategyBase` much easier (because `Hint` would validate its own constructor)
* You could have a type of `Line` to represent the cells
    * Similar notes about `Hint` would also apply to `Line`
    * This would probably be a `record struct`
    * Rather than converting to a `string` you would probably convert to `ReadOnlySpan<char>` and let the consumer decide when to materialize it
    * You could define equality to `string` for e.g. `actualLine.Should().Be("100_0_1");`
* If you had multiple ways of representing data (arrays, `strings`, and `Hint` and `Line`) it would make sense to have members on the `IStrategy` interface that accept these. They could be default interface implementations

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