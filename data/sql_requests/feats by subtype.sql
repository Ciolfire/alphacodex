SELECT feat_subtype.name , COUNT(feat.id) as "number of feats" FROM feat
LEFT JOIN feat_subtype
ON feat_subtype.id = feat.subtype
GROUP BY subtype
ORDER BY "number of feats"