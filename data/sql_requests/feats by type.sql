SELECT feat_type.name , COUNT(feat.id) as "number of feats" FROM feat
LEFT JOIN feat_type
ON feat_type.id = feat.type
GROUP BY type
ORDER BY "number of feats" DESC