UPDATE feat_prerequisite
SET parent_id = (SELECT feat.current
									FROM feat
									WHERE feat.id = feat_prerequisite.parent_id)
WHERE (SELECT feat.current 
				FROM feat
				WHERE feat.id = feat_prerequisite.parent_id) != 0
