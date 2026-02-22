# Task 003: Compute differences and generate binary hash

In the `ComputePerceptualHash` method, calculate the horizontal differences between adjacent pixels in each row, threshold them to binary values (0 or 1), and pack the 64 binary values into a 64-bit unsigned integer.

## Deliverable
- Differences computed: for each row, compare pixel intensities.
- Binary array created.
- Packed into ulong and returned.