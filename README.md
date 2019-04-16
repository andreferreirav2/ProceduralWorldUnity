# General Notes

## Performance metrics

| Phase of Test / Faces                       | 20 | 80 | 320 | 1280 |  5120 | 20480 | 81920 | 327680 |
|---------------------------------------------|:--:|:--:|:---:|:----:|:-----:|:-----:|:-----:|:------:|
|                                             |  5 |  9 | 121 | 1894 | 30619 |       |       |        |
| After removal of .Equals() from addAdjacent |  1 |  3 |  43 |  582 |  9366 |       |       |        |
| Fail Fast for vertex comparison             |  1 |  3 |  40 |  580 |  9279 |       |       |        |
| Map triangles from vertex dict              |  1 |  1 |  5  |  19  |   78  |  280  | 1144  | 4461   |
